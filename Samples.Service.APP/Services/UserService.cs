using Infrastructure.CacheManager;
using Infrastructure.DEncrypt;
using Infrastructure.Enums;
using Infrastructure.Models;
using Infrastructure.Utilities;
using Samples.Repository.Enums;
using Samples.Repository.Interface;
using Samples.Repository.Models;
using Samples.Service.APP.BaseProvider;
using Samples.Service.APP.Dto;
using Samples.Service.APP.Interface;
using Samples.Service.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samples.Service.APP.Services
{
    /// <summary>
    /// 
    /// </summary>
    public partial class UserService : BaseSerivceDomin<McUser, IBookStoreRepository<McUser>>, IUserService
    {
        private readonly ICommonService _commonService;
        private readonly IHttpClientService _httpClientService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="commonService"></param>
        public UserService(IUserDomainService repository, ICommonService commonService, IHttpClientService httpClientService) : base(repository)
        {
            _commonService = commonService;
            _httpClientService = httpClientService;
        }
        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="rq"></param>
        /// <returns></returns>
        public async Task<ResultDto<bool>> SendValidCodeAsync(ValidCodeRQ rq)
        {
            //1.手机号,2邮箱
            var receiverType = 0;
           var s= CacheContext.Cache.Exists($"SMSCode");
            rq.Receiver = rq.Receiver.Trim().ToLower();//统一转为小写
            if (RegexHelper.Check(rq.Receiver, EnumPattern.Email))
            {
                receiverType = 2;
                if (rq.Receiver.Length > 50) return Result<bool>(ResponseCode.sys_param_format_error, "邮箱长度不能大于50");
            }
            else if (rq.AreaCode == "86" && RegexHelper.Check(rq.Receiver, EnumPattern.Mobile))
            {
                receiverType = 1;
            }
            else if (rq.AreaCode == "852" || rq.AreaCode == "853" || rq.AreaCode == "886")
            {
                receiverType = 1;
            }
            return Result(true);

        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="rq"></param>
        /// <returns></returns>
        public ResultDto<RegisterRP> Register(RegisterRQ rq)
        {
            var result = new ResultDto<RegisterRP>(ResponseCode.sys_fail, "注册失败");
            rq.Name = rq.Name.Trim().ToLower();
            if (!new[] { 1, 2 }.Contains(rq.Type)) { return Result<RegisterRP>(ResponseCode.sys_param_format_error, "注册类型错误"); }
            if (rq.Type == 1)
            {
                if (!RegexHelper.CheckMobile(rq.Name, rq.AreaCode)) return Result<RegisterRP>(ResponseCode.sys_param_format_error, "手机号码格式不正确");
            }
            else
            {
                if (!RegexHelper.Check(rq.Name, EnumPattern.Email)) return Result<RegisterRP>(ResponseCode.sys_param_format_error, "邮箱格式不正确");
                if (rq.Name.Length > 50) return Result<RegisterRP>(ResponseCode.sys_param_format_error, "邮箱长度不能大于50");
            }
            if (string.IsNullOrWhiteSpace(rq.Pwd))
            {
                return Result<RegisterRP>(ResponseCode.sys_param_format_error, "登录密码不能为空");
            }

            if (!RegexHelper.Check(rq.Pwd, EnumPattern.Password))
            {
                return Result<RegisterRP>(ResponseCode.sys_param_format_error, "登录密码须为8-24位数字和字母组成");
            }
            if (rq.Pwd != rq.PwdConfirm)
            {
                return Result<RegisterRP>(ResponseCode.sys_verify_failed, "前后密码不一致");
            }
            if (string.IsNullOrWhiteSpace(rq.Code))
            {
                return Result<RegisterRP>(ResponseCode.sys_verify_failed, "请输入接收到的验证码");
            }

            var isSave = false;
            var pwd = AESEncrypt.Encrypt(rq.Pwd, AESEncrypt.pwdKey);
            var receiverEncrypt = AESEncrypt.Encrypt(rq.Name, AESEncrypt.infoKey);
            if (repository.IsExist(q => (q.Mobile == receiverEncrypt || q.Email == receiverEncrypt) && q.DataStatus != (byte)EnumDataStatus.Delete))
            {
                return Result<RegisterRP>(ResponseCode.sys_verify_failed, "账号已存在");
            }
            var uid = Guid.NewGuid().ToString();
            var user = new McUser
            {
                Uid = uid,
                Mobile = rq.Type == 1 ? receiverEncrypt : string.Empty,
                AreaCode = rq.Type == 1 ? rq.AreaCode : string.Empty,
                Email = rq.Type == 2 ? receiverEncrypt : string.Empty,
                Pwd = pwd,
                CountryId = 0,
                DataStatus = (byte)EnumDataStatus.Enable,
                Gender = 0,
                Name = string.Empty,
                CreateTime = DateTime.Now,
                Ip = HttpContext.GetIp ?? string.Empty,
                Account = 0,
            };
            repository.Add(user);

            isSave = repository.SaveChanges() > 0;
            result.Res = 1;
            result.Msg = "注册成功";
            result.Ec = ResponseCode.sys_success;
            return result;
        }

        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <returns></returns>
        public async Task<ResultDto<UserInfoRP>> GetUserInfoAsync(string uid)
        {

            var result = new UserInfoRP
            {
                Email = string.Empty,
                Mobile = string.Empty,
                IsLogin = true,
            };
            if (string.IsNullOrEmpty(uid))
            {
                return Result(result);
            }
            var user = await repository.FindSingleAsync<McUser>(q => q.Uid == uid && q.DataStatus == (byte)EnumDataStatus.Enable);

            if (user == null)
            {
                return Result(result);
            }
            result.UID = user.Uid;
            result.Account = user.Account;
            result.Name = user.Name;
            result.Mobile = String.IsNullOrEmpty(user.Mobile) ? String.Empty : AESEncrypt.Decrypt(user.Mobile, AESEncrypt.infoKey);
            result.AreaCode = user.AreaCode;
            result.Email = String.IsNullOrEmpty(user.Email) ? String.Empty : AESEncrypt.Decrypt(user.Email, AESEncrypt.infoKey);

            return Result(result);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="rq"></param>
        /// <returns></returns>
        public async Task<ResultDto<UserInfoRP>> LoginAsync(UserLoginRQ rq)
        {
            McUser user = new McUser();
            // 1.手机号,2邮箱
            var receiverType = RegexHelper.Check(rq.Name, EnumPattern.Email) == true ? 2 : 1;
            if (rq.Type == (byte)LoginType.VCode)
            {
                if (string.IsNullOrWhiteSpace(rq.Pwd) == true) return Result<UserInfoRP>(ResponseCode.sys_verify_failed, "请输入接收到的验证码");
                if (_commonService.CheckValidationCode(rq.Name, rq.Pwd, rq.AreaCode, out string errmsg) == false) return Result<UserInfoRP>(ResponseCode.sys_verify_failed, errmsg);
            }
            var receiverEncrypt = AESEncrypt.Encrypt(rq.Name, AESEncrypt.infoKey);
            var encryptPwd = AESEncrypt.Encrypt(rq.Pwd, AESEncrypt.pwdKey);
            if (receiverType == 2)
            {
                //兼容小写登录
                var receiver_LowerEncryptLower = AESEncrypt.Encrypt(rq.Name.Trim().ToLower(), AESEncrypt.infoKey);
                user = await repository.FindSingleAsync<McUser>(q => (q.Email == receiverEncrypt || q.Email == receiver_LowerEncryptLower) && q.DataStatus != (byte)EnumDataStatus.Delete);
            }
            else
            {
                user = await repository.FindSingleAsync<McUser>(q => (q.Mobile == receiverEncrypt && q.AreaCode == rq.AreaCode) && q.DataStatus != (byte)EnumDataStatus.Delete);
            }
            if (user == null && rq.Type == (byte)LoginType.Pwd) return Result<UserInfoRP>(ResponseCode.sys_fail, "请先注册");

            var result = await GetUserInfoAsync(user.Uid);


            return result;
        }
     
    }
}

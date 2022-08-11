using Infrastructure.CacheManager;
using Infrastructure.Config;
using Infrastructure.Utilities;
using Infrastructure.Utilities.PDFReport;
using Samples.Service.APP.BaseProvider;
using Samples.Service.APP.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Samples.Service.APP.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class CommonService : BaseSerivce, ICommonService
    {
        /// <summary>
        /// 检查验证码
        /// </summary>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <param name="areaCode"></param>
        /// <param name="errmsg"></param>
        /// <returns></returns>
        public bool CheckValidationCode(string name, string code, string areaCode, out string errmsg)
        {
            name = name.Trim().ToLower();//检查统一转换为小写
            var receiverType = 1;
            errmsg = string.Empty;
            if (RegexHelper.Check(name, EnumPattern.Email)) receiverType = 2;
            if (!string.IsNullOrEmpty(areaCode) && receiverType == 1) name = $"{areaCode}{name}";
            if (CacheContext.Cache.Exists($"SMSCode:{name}"))
            {
                if (CacheContext.Cache.Get($"SMSCode:{name}") == code)
                {
                    CacheContext.Cache.Remove($"SMSCode:{name}");

                    return true;
                }
                if (code == "12345" && AppSetting.GetConfigBoolean("Samples:IsDevelopment")) return true;//通用万能验证码
                errmsg = "验证码错误，请重新输入";
                return false;
            }
            if (code == "12345" && AppSetting.GetConfigBoolean("Samples:IsDevelopment")) return true;//通用万能验证码
            errmsg = "验证码已过期，请重新获取";
            return false;
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="rq"></param>
        /// <returns></returns>
        public void PdfDemo()
        {
            string tempFilePath = @"D:\demo\test1.pdf";
            string createdPdfPath = @"D:\demo\test2.pdf";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            PdfHelper.PutContentV2(tempFilePath, createdPdfPath, parameters);
        }
    }
}

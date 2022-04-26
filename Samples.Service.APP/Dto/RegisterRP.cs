using System;
using System.Collections.Generic;
using System.Text;

namespace Samples.Service.APP.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class RegisterRP
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string Uid { get; set; }
        /// <summary>
        /// 平台账号
        /// </summary>
        public int Account { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class UserInfoRP
    {
        /// <summary>
        /// uid
        /// </summary>
        public string UID { get; set; }
        /// <summary>
        ///是否登录
        /// </summary>
        public bool IsLogin { get; set; }
        /// <summary>
        /// 平台账号 没有默认传:0
        /// </summary>
        public int Account { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 区号(中国大陆 86,中国香港 852,中国澳门 853,中国台湾886)
        /// </summary>
        public string AreaCode { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }
    }
}

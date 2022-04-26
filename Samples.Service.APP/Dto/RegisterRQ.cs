using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Samples.Service.APP.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class RegisterRQ
    {
        /// <summary>
        /// 账号(手机号,邮箱)
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        ///手机号必填 区号(中国大陆 86,中国香港 852,中国澳门 853,中国台湾886)
        /// </summary>
        [Required]
        public string AreaCode { get; set; }
        /// <summary>
        /// 1.手机号注册,2.邮箱注册
        /// </summary>
        [Range(1, 2)]
        [Required]
        public int Type { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string Pwd { get; set; }
        /// <summary>
        /// 确认密码
        /// </summary>
        [Required]
        public string PwdConfirm { get; set; }
        /// <summary>
        /// 验证码 (测试可以填12345)
        /// </summary>
        [Required]
        public string Code { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class UserLoginRQ
    {
        /// <summary>
        ///登录类型 (1:密码登录;2:验证码登录)
        /// </summary>
        [Range(1, 2)]
        [Required]
        public byte Type { get; set; }
        /// <summary>
        /// 账号(手机号,邮箱)
        /// </summary>
        [Required]
        [JsonProperty("Name")]
        public string Name { get; set; }
        /// <summary>
        /// 密码/验证码
        /// </summary>
        [Required]
        [JsonProperty("Pwd")]
        public string Pwd { get; set; }
        /// <summary>
        /// 区号
        /// </summary>
        public string AreaCode { get; set; } = "86";
        /// <summary>
        /// 设备名称 
        /// </summary>
        public string DeviceName { get; set; }
        /// <summary>
        /// 设备系统
        /// </summary>
        public string DeviceSystem { get; set; }
        /// <summary>
        /// 设备型号 
        /// </summary>
        public string DeviceType { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ValidCodeRQ
    {
        /// <summary>
        /// 接收手机/邮箱
        /// </summary>
        [Required]
        public string Receiver { get; set; }
        /// <summary>
        ///手机号必填 区号(中国大陆 86,中国香港 852,中国澳门 853,中国台湾886)
        /// </summary>
        public string AreaCode { get; set; }
        /// <summary>
        /// 图形验证码
        /// </summary>
        //public string PicCode { get; set; }
    }

}
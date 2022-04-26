using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Infrastructure.Utilities
{
    /// <summary>
    /// 正则验证枚举
    /// </summary>
    public enum EnumPattern
    {
        /// <summary>
        /// 密码(长度为6-12位)
        /// </summary>
        Password = 0,
        /// <summary>
        /// 手机号码
        /// </summary>
        Mobile = 1,
        /// <summary>
        /// 邮箱
        /// </summary>
        Email = 2,
        /// <summary>
        /// 15位身份证
        /// </summary>
        IDCard15 = 3,
        /// <summary>
        /// 18位身份证
        /// </summary>
        IDCard18 = 4,
        /// <summary>
        /// 交易账号
        /// </summary>
        Account = 5,
        /// <summary>
        /// 姓名
        /// </summary>
        Name = 6,
        /// <summary>
        /// 内部订单号
        /// </summary>
        billno = 7,
        /// <summary>
        /// 外部订单号
        /// </summary>
        ordernum = 8,
        /// <summary>
        /// QQ号
        /// </summary>
        QQ = 9,
        /// <summary>
        /// 护照
        /// </summary>
        Passport = 10,
        /// <summary>
        /// 证件号码
        /// </summary>
        CredentialsNum = 11,
        /// <summary>
        /// 浮点数
        /// </summary>
        DecimalNum = 12
    }

    /// <summary>
    /// 正则验证帮助类
    /// </summary>
    public class RegexHelper
    {
        /// <summary>
        /// 正则表达式
        /// </summary>
        private static string[] regArray =
        {
            @"^(?=.*[a-zA-Z])(?=.*\d)[a-zA-Z\d]{8,24}$",
            @"^1[3|4|5|6|7|8|9][0-9]\d{8}$",//@"^\d{8,15}$", 
            @"^([a-zA-Z0-9]+[_|_|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|_|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,4}$",
            //@"^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$",
            //@"^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])((\d{4})|\d{3}[a-zA-Z])$" ,
            @"^[1-9]\d{7}((0[1-9])|(1[0-2]))((0[1-9]|[1|2]\d)|3[0-1])\d{3}$",
            @"^[1-9]\d{5}[1-9]\d{3}((0[1-9])|(1[0-2]))((0[1-9]|[1|2]\d)|3[0-1])((\d{4})|\d{3}[a-zA-Z])$" ,
            @"^\d{6,8}$",
            @"^[a-zA-Z-\u0391-\uFFE5]+$",
            @"^[(SP_)|(yeepay_)|(upop_)|(\w{2})]\w{18,}$",
            @"^(XPay)\w{32}$",
            @"^\d{5,12}$",
            @"^[A-Za-z0-9]{5,30}$",
            @"[\u4E00-\u9FA5]",
            @"^(\d*\.)?\d+$"
        };

        /// <summary>
        /// 自定义正则验证
        /// </summary>
        /// <param name="checkText">需要验证的文本</param>
        /// <param name="pattern">正则表达式</param>
        /// <returns></returns>
        public static bool Check(string checkText, EnumPattern pattern)
        {
            return new Regex(regArray[(int)pattern]).IsMatch(checkText);
        }
        /// <summary>
        /// 自定义正则验证-验证手机号
        /// </summary>
        /// <param name="checkText"></param>
        /// <param name="areaCode"></param>
        /// <returns></returns>
        public static bool CheckMobile(string checkText, string areaCode)
        {
            if (areaCode == "86") return new Regex(regArray[(int)EnumPattern.Mobile]).IsMatch(checkText);

            return true;
        }
        //只保留字符串数字
        public static string GetNumberAlpha(string source)
        {
            string pattern = "[0-9]";
            string strRet = "";
            MatchCollection results = Regex.Matches(source, pattern);
            foreach (var v in results)
            {
                strRet += v.ToString();
            }
            return strRet;
        }
    }
}

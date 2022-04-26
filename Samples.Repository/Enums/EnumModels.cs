using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Samples.Repository.Enums
{
    /// <summary>
    /// 数据状态
    /// </summary>
    public enum EnumDataStatus
    {
        /// <summary>
        /// 未启用
        /// </summary>
        [Description("未启用")]
        None = 0,
        /// <summary>
        /// 启用
        /// </summary>
        [Description("启用")]
        Enable = 1,
        /// <summary>
        /// 停用
        /// </summary>
        [Description("停用")]
        Disable = 2,
        /// <summary>
        /// 删除
        /// </summary>
        [Description("删除")]
        Delete = 3
    }
    public enum EnumPlatformType : byte
    {
        /// <summary>
        /// 全部
        /// </summary>
        [Description("全部")]
        All = 0,
        /// <summary>
        /// PC
        /// </summary>
        [Description("PC")]
        PC = 1,
        /// <summary>
        /// WAP
        /// </summary>
        [Description("WAP")]
        WAP = 2,
        /// <summary>
        /// IOS
        /// </summary>
        [Description("IOS")]
        IOS = 3,
        /// <summary>
        /// ANDROID
        /// </summary>
        [Description("ANDROID")]
        ANDROID = 4
    }
    public enum LoginType : byte
    {
        /// <summary>
        /// 密码登录
        /// </summary>
        [Description("密码登录")]
        Pwd = 1,
        /// <summary>
        /// 验证码登录
        /// </summary>
        [Description("验证码登录")]
        VCode = 2
    }
}

using AutoMapper;

using System;

namespace Samples.Service.APP.AutoMapper
{
    /// <summary>
    /// 
    /// </summary>
    public class ViewModelToDomainMappingProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public ViewModelToDomainMappingProfile()
        {
            CreateMap <ChattingRetryAddCustomerParamDto, ChattingRetryAddCustomerDto> ()
                .ConstructUsing(c => new ChattingRetryAddCustomerDto() { identity="dd" });

        }
    }

    /// <summary>
    /// 机器人被删好友重新发起好友请求 入参
    /// </summary>
    public class ChattingRetryAddCustomerParamDto
    {
        /// <summary>
        /// 招呼语
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 业务流水号 好友id
        /// </summary>
        public string relaSerialNo { get; set; }

        /// <summary>
        /// 机器人编号
        /// </summary>
        public string wxId { get; set; }
        /// <summary>
        /// 好友备注
        /// Default:
        /// Nullable:True
        /// </summary>
        public string vcFriendRemark { get; set; }
        /// <summary>
        /// 好友描述
        /// </summary>
        public string vcFriendDescription { get; set; }
    }
    /// <summary>
    /// 机器人被删好友重新发起好友请求 入参类
    /// </summary>
    public class ChattingRetryAddCustomerDto 
    {
        /// <summary>
        /// 招呼语
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 业务流水号 好友id
        /// </summary>
        public string relaSerialNo { get; set; }

        /// <summary>
        /// 机器人编号
        /// </summary>
        public string wxId { get; set; }
        /// <summary>
        /// 合作方标识
        /// </summary>
        public string identity { get; set; }
        /// <summary>
        /// 商家编号
        /// </summary>
        public string merchatId { get; set; }
        /// <summary>
        /// 操作编号
        /// </summary>
        public string serialNo { get; set; }
    }
}

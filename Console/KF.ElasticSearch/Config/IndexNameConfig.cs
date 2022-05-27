using System;
using System.Collections.Generic;
using System.Text;

namespace KF.ElasticSearch.Config
{
    /// <summary>
    /// ElasticSearch 索引管理类
    /// </summary>
    public class IndexNameConfig
    {
        #region 索引
        /// <summary>
        /// 客服会话列表索引
        /// </summary>
        public static string Index_UserList = "userlist";

        /// <summary>
        /// 商家会话列表索引
        /// </summary>
        public static string Index_MerUserList = "mer_userlist";

        /// <summary>
        /// 操作员会话列表索引
        /// </summary>
        public static string Index_OpUserList = "op_userlist";

        /// <summary>
        /// 客服私聊聊天记录
        /// </summary>
        public static string Index_UserMessage = "usermessage";

        /// <summary>
        /// 客服群聊聊天记录
        /// </summary>
        public static string Index_UserChatRoomMessage = "userchatroommessage";

        /// <summary>
        /// 客服直播间消息聊天记录  别名：SQ_LiveRoomMessage
        /// </summary>
        public static string Index_LiveRoomMessage = "liveroommessage";

        /// <summary>
        /// 客服系统小商店消息聊天记录
        /// </summary>
        public static string Index_MiniShopMessage = "minishopmessage";
        #endregion

        #region 别名

        /// <summary>
        /// 客服会话列表索引  别名：SQ_UserListMongo
        /// </summary>
        public static string Alias_UserList = "SQ_UserListMongo";

        /// <summary>
        /// 商家会话列表索引  别名：SQ_MerUserListMongo
        /// </summary>
        public static string Alias_MerUserList = "SQ_MerUserListMongo";

        /// <summary>
        /// 操作员会话列表索引  别名：SQ_OPUserListMongo
        /// </summary>
        public static string Alias_OpUserList = "SQ_OPUserListMongo";

        /// <summary>
        /// 客服私聊聊天记录  别名：SQ_UserMessage
        /// </summary>
        public static string Alias_UserMessage = "SQ_UserMessage";

        /// <summary>
        /// 客服群聊聊天记录  别名：SQ_UserChatRoomMessage
        /// </summary>
        public static string Alias_UserChatRoomMessage = "SQ_UserChatRoomMessage";

        /// <summary>
        /// 客服直播间消息聊天记录  别名：SQ_LiveRoomMessage
        /// </summary>
        public static string Alias_LiveRoomMessage = "SQ_LiveRoomMessage";

        /// <summary>
        /// 客服系统小商店消息聊天记录 别名 SQ_MiniShopMessage
        /// </summary>
        public static string Alias_MiniShopMessage = "SQ_MiniShopMessage";
        #endregion

        /// <summary>
        /// 根据前缀和时间获取索引名称
        /// </summary>
        /// <param name="vcIndex">索引名称</param>
        /// <param name="vcTime">业务实际</param>
        /// <param name="vcMerChantNo">商家编号</param>
        /// <returns></returns>
        public static string GetIndexName(string vcIndex, string vcTime)
        {
            vcTime = Convert.ToDateTime(vcTime).ToString("yyyy.MM");
            if (!string.IsNullOrEmpty(vcIndex) && !string.IsNullOrEmpty(vcTime))
            {
                //return "kefu_" + vcIndex + "_" + vcTime;
                return vcIndex + vcTime;
            }
            return null;
        }
    }
}

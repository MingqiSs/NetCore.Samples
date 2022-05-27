using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticsearchDemo
{
    public class UserListSearchResultDto
    {
        public int nRowIndex { get; set; }
        public string vcUser { get; set; }

        public int snType { get; set; }

        public string vcName { get; set; }

        public long snParentId { get; set; }

        public string vcParentName { get; set; }

        /// <summary>
        /// 会话消息数 （会话开始时间-会话结束时间）
        /// </summary>
        public long nMessageCount { get; set; }
    }
}

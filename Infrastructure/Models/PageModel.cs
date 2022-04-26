using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Models
{
    /// <summary>
    /// 分页列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageModel
    {
        /// <summary>
        /// 表
        /// </summary>
        public string Table { get; set; }
        /// <summary>
        /// 字段
        /// </summary>
        public string Query { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public string Order { get; set; }
        /// <summary>
        /// 条件
        /// </summary>
        public string Where { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Pageindex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int PageSize { get; set; }
    }
    public class PageCount
    {
        /// <summary>
        /// 
        /// </summary>
        public int Count { get; set; }
      
    }
}

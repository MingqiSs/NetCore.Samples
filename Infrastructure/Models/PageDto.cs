using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Models
{
    /// <summary>
    /// 分页列表(For API)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageDto
    {
        /// <summary>
        /// 当前页面索引
        /// </summary>
        public int idx { get; set; }

        /// <summary>
        /// 页面数据数量
        /// </summary>
        public int ps { get; set; }

        /// <summary>
        /// 全部数据总数
        /// </summary>
        public long dc { get; set; }

        /// <summary>
        /// 页面总数
        /// </summary>
        public int pt
        {
            get
            {
                return ps > 0 ? (int)Math.Ceiling((double)dc / ps) : 0;
            }
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageDto<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public PageDto pg { get; set; }

        /// <summary>
        /// 分页列表
        /// </summary>
        public List<T> lst { get; set; }
        /// <summary>
        /// 其他数据
        /// </summary>
        public object data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        public PageDto(int pageIndex, int pageSize)
        {
            pg = new PageDto { idx = pageIndex, ps = pageSize };
        }

    }
}

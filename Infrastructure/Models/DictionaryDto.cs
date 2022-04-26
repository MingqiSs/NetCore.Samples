using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Models
{
   public class DictionaryDto
    {
        /// <summary>
        /// 字典名称
        /// </summary>
        public string DicName { get; set; }
        /// <summary>
        /// 字典号
        /// </summary>
        public string DicNo { get; set; }
        /// <summary>
        /// 配置
        /// </summary>
        public string Config { get; set; }
        /// <summary>
        /// 配置值
        /// </summary>
        public List<DicValue> Data { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class DicValue
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// IODB ID映射值
        /// </summary>
        public string MapValue { get; set; }
        /// <summary>
        ///序号
        /// </summary>
       // public int OrderNo { get; set; }
    }
}

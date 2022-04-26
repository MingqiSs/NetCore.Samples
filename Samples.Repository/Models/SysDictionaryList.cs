using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Samples.Repository.Models
{
    [Table("Sys_DictionaryList")]
    public partial class SysDictionaryList
    {
        [Key]
        [Column("DicList_ID")]
        public int DicListId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateDate { get; set; }
        [Column("CreateID")]
        public int? CreateId { get; set; }
        [Column(TypeName = "varchar(30)")]
        public string Creator { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string DicName { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string DicValue { get; set; }
        [Column("Dic_ID")]
        public int? DicId { get; set; }
        public sbyte? Enable { get; set; }
        [Column(TypeName = "varchar(30)")]
        public string Modifier { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifyDate { get; set; }
        [Column("ModifyID")]
        public int? ModifyId { get; set; }
        public int? OrderNo { get; set; }
        [Column(TypeName = "varchar(2000)")]
        public string Remark { get; set; }
    }
}

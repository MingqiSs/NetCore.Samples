using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Samples.Repository.Models
{
    [Table("Sys_Dictionary")]
    public partial class SysDictionary
    {
        [Key]
        [Column("Dic_ID")]
        public int DicId { get; set; }
        [Column(TypeName = "varchar(4000)")]
        public string Config { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateDate { get; set; }
        [Column("CreateID")]
        public int? CreateId { get; set; }
        [Column(TypeName = "varchar(30)")]
        public string Creator { get; set; }
        [Column("DBServer", TypeName = "varchar(4000)")]
        public string Dbserver { get; set; }
        [Column(TypeName = "varchar(4000)")]
        public string DbSql { get; set; }
        [Required]
        [Column(TypeName = "varchar(100)")]
        public string DicName { get; set; }
        [Required]
        [Column(TypeName = "varchar(100)")]
        public string DicNo { get; set; }
        public sbyte Enable { get; set; }
        [Column(TypeName = "varchar(30)")]
        public string Modifier { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifyDate { get; set; }
        [Column("ModifyID")]
        public int? ModifyId { get; set; }
        public int? OrderNo { get; set; }
        public int ParentId { get; set; }
        [Column(TypeName = "varchar(2000)")]
        public string Remark { get; set; }
    }
}

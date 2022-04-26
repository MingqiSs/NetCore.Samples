using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Samples.Repository.Models
{
    [Table("MC_User")]
    public partial class McUser
    {
        [Key]
        [Column("UID", TypeName = "varchar(40)")]
        public string Uid { get; set; }
        public int Account { get; set; }
        [Required]
        [Column(TypeName = "varchar(200)")]
        public string Mobile { get; set; }
        [Required]
        [Column(TypeName = "varchar(500)")]
        public string Email { get; set; }
        [Required]
        [Column(TypeName = "varchar(200)")]
        public string Pwd { get; set; }
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; }
        public byte DataStatus { get; set; }
        public byte Gender { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Birthdate { get; set; }
        [Column("CountryID")]
        public int CountryId { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string Remark { get; set; }
        [Required]
        [Column("IP", TypeName = "varchar(50)")]
        public string Ip { get; set; }
        [Required]
        [Column(TypeName = "varchar(10)")]
        public string AreaCode { get; set; }
        [Column(TypeName = "timestamp")]
        public DateTime CreateTime { get; set; }
    }
}

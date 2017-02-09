//
//  Souce	Path	Dict.cs
//  create	Date	2017-2-8 11:13:51		
//  created	By	Ares.Zhao
//
using System;
using System.ComponentModel.DataAnnotations;

namespace QuakeViewer.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int? UserType { get; set; }
        public int? status { get; set; }
        public int? AccountType { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}

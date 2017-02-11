//
//  Souce	Path	Choise.cs
//  create	Date	2017-2-9 10:40:28		
//  created	By	Ares.Zhao
//
using System;
using System.ComponentModel.DataAnnotations;
namespace QuakeViewer.Models
{
    public class Choice
    {
        [Key]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FirstChoice { get; set; }
        public string SecondChoice { get; set; }
        public string ThirdChoice { get; set; }
        public string ForthChoice { get; set; }
        public string FifthChoice { get; set; }
        public int? FromType { get; set; }
        public int? MinorResult { get; set; }
        public int? MajorResult { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}

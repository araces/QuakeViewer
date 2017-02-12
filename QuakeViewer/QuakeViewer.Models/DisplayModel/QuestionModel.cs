//
//  Souce	Path	QuestionModel.cs
//  create	Date	2017-2-12 14:42:28		
//  created	By	Ares.Zhao
//
using System;
namespace QuakeViewer.Models
{
    public class QuestionModel
    {
        public string UserId { get; set; }

        public string Provincial { get; set; }
        public string City { get; set; }
        public string Region { get; set; }

        public int BuildLevel { get; set; }
        public int StructLevel { get; set; }
        public int Designed { get; set; }
        public int Jobstatus { get; set; }
        public int YearLevel { get; set; }
    }
}

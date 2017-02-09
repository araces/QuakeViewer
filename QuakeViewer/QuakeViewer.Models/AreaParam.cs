//
//  Souce	Path	Sample.cs
//  create	Date	2017-2-7 18:24:9		
//  created	By	Ares.Zhao
//
using System;
using System.ComponentModel.DataAnnotations;

namespace QuakeViewer.Models
{
    public class AreaParam
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
        public int? GroupNo { get; set; }
        public int? SiteType { get; set; }
        public int? IntensityDegree { get; set; }
    }
}

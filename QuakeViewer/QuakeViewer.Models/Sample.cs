//
//  Souce	Path	Sample.cs
//  create	Date	2017-2-7 18:24:9		
//  created	By	Ares.Zhao
//
using System;
using System.ComponentModel.DataAnnotations;

namespace QuakeViewer.Models
{
    public class Sample
    {
        [Key()]
        public int? First { get; set; }
        public string Name { get; set; }
    }
}

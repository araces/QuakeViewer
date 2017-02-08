//
//  Souce	Path	Dict.cs
//  create	Date	2017-2-8 11:13:51		
//  created	By	Ares.Zhao
//
using System;
using System.ComponentModel.DataAnnotations;

namespace QuakeViewer.Models
{
    public class Dict
    {


        [Key]
        public int Keys { get; set; }
        public string Values { get; set; }
    }
}

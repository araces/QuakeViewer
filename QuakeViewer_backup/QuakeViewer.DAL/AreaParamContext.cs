//
//  Souce	Path	AreaParamContext.cs
//  create	Date	2017-2-11 15:19:42		
//  created	By	Ares.Zhao
//
using System;
using System.Data.Entity;
using QuakeViewer.Models;
namespace QuakeViewer.DAL
{
    public class AreaParamContext : DbContext
    {
        public AreaParamContext() : base("name=defaultConnectionString")
        {
        }


        public DbSet<AreaParam> AreaParams { get; set; }

    }
}

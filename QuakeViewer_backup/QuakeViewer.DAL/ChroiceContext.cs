//
//  Souce	Path	ChroiceContext.cs
//  create	Date	2017-2-11 15:20:51		
//  created	By	Ares.Zhao
//
using System;
using System.Data.Entity;
using QuakeViewer.Models;

namespace QuakeViewer.DAL
{
    public class ChroiceContext : DbContext
    {
        public ChroiceContext() : base("name=defaultConnectionString")
        {
        }

        public DbSet<Choice> Choices { get; set; }
    }
}

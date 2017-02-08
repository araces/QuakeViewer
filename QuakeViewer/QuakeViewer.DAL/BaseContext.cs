//
//  Souce	Path	MyClass.cs
//  create	Date	2017-2-8 11:28:18		
//  created	By	Ares.Zhao
//
using System;
using System.Data.Entity;
using QuakeViewer.Models;

namespace QuakeViewer.DAL
{
    public class BaseContext : DbContext
    {
        public BaseContext() : base("name=defaultConnectionString")
        {
        }

        public DbSet<Dict> Dict { get; set; }
    }
}

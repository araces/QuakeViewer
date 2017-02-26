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
    public class AccountContext : DbContext
    {
        public AccountContext() : base("name=defaultConnectionString")
        {
        }

        public DbSet<Account> Accounts { get; set; }

    }
}

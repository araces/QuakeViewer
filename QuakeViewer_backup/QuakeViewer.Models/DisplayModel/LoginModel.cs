//
//  Souce	Path	LoginModel.cs
//  create	Date	2017-2-9 11:15:7		
//  created	By	Ares.Zhao
//
using System;
namespace QuakeViewer.Models
{
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public string RegistUserName { get; set; }
        public string RegistPassword { get; set; }
        public string RegistConfirmPassword { get; set; }
        public string Mobile { get; set; }

        public bool HasError { get; set; }
    }
}

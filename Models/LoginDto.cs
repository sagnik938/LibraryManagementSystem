using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELibraryManagement.Models
{
    public class LoginDto
    {
        public string Username {get; set;}
        public string Password { get; set; }

        public LoginDto(string username , string password)
        {
            this.Username = username;
            this.Password = password;
        }
    }
}
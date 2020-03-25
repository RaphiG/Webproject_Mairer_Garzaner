using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webproject_M_R.Models
{
    public class UserLogin
    {

        public string Username { get; set; }
        public string Password { get; set; }

        public UserLogin() : this("", "") { }

        public UserLogin(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }

        public override string ToString()
        {
            return Username + " " + Password;
        }
    }
}
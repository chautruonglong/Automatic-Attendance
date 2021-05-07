using System;
using System.Collections.Generic;
using System.Text;

namespace AutoAttendant.Models
{
    public class UserTemp
    {
        string email;
        string username;
        string password;


        public string Email
        {
            get
            {
                return this.email;
            }
            set
            {
                this.email = value;
            }
        }
        public string Username
        {
            get
            {
                return this.username;
            }
            set
            {
                this.username = value;
            }
        }

        public string Password
        {
            get
            {
                return this.password;
            }
            set
            {
                this.password = value;
            }
        }

        public UserTemp()
        {

        }

        public UserTemp(string Email, string Username, string Password)
        {
            this.Email = Email;
            this.Username = Username;
            this.Password = Password;
        }
    }
}

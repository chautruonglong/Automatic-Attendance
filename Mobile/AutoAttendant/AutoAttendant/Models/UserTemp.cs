using System;
using System.Collections.Generic;
using System.Text;

namespace AutoAttendant.Models
{
    public class UserTemp
    {
        string email;
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

        public UserTemp(string Email, string Password)
        {
            this.Email = Email;
            this.Password = Password;
        }
    }
}

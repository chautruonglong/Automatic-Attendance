using System;
using System.Collections.Generic;
using System.Text;

namespace AutoAttendant.Models
{
    public class UserTemp
    {
        string _email;
        string _password;


        public string email
        {
            get
            {
                return this._email;
            }
            set
            {
                this._email = value;
            }
        }

        public string password
        {
            get
            {
                return this._password;
            }
            set
            {
                this._password = value;
            }
        }


        public UserTemp()
        {

        }

        public UserTemp(string Email, string Password)
        {
            this.email = Email;
            this.password = Password;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace AutoAttendant.Models
{
    class User
    {
        int id;
        string username;
        string password;

        public int Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
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

        public User()
        {

        }

        public User(string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;
        }
        public bool CheckLogin()
        {
            if (this.Username == this.Password)
            {
                return true;
            }
            else return false;
        }
    }
}

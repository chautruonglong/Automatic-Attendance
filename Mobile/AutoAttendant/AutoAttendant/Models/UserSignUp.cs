using System;
using System.Collections.Generic;
using System.Text;

namespace AutoAttendant.Models
{
    public class UserSignUp
    {
        private string _name;
        private string _phone;
        private string _faculty;
        private string _email;
        private string _password;

        public UserSignUp(string name, string phone, string degree, string email, string password)
        {
            _name = name;
            _phone = phone;
            _faculty = degree;
            _email = email;
            _password = password;
        }

        public string name { get => _name; set => _name = value; }
        public string phone { get => _phone; set => _phone = value; }
        public string faculty { get => _faculty; set => _faculty = value; }
        public string email { get => _email; set => _email = value; }
        public string password { get => _password; set => _password = value; }
    }
}

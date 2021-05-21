using System;
using System.Collections.Generic;
using System.Text;

namespace AutoAttendant.Models
{

    public class UserVu
    {
        string _email;
        string _password;
        Tokens _tokens;

        public UserVu(string email, string password)
        {
            _email = email;
            _password = password;
        }

        public string email { get => _email; set => _email = value; }
        public string password { get => _password; set => _password = value; }
        public Tokens tokens { get => _tokens; set => _tokens = value; }
        //public List<string> tokens { get => _tokens; set => _tokens = value; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace AutoAttendant.Models
{
    class UserTempSignUp
    {
        //int _idLecture;
        string _email;
        string _username;
        string _password;

        public UserTempSignUp(string email,string username, string password)
        {
            //_idLecture = idLecture;
            _email = email;
            _username = username;
            _password = password;
        }

        //public int idLecture { get => _idLecture; set => _idLecture = value; }
        public string email { get => _email; set => _email = value; }
        public string username { get => _username; set => _username = value; }
        public string password { get => _password; set => _password = value; }
    }
}

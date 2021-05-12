using System;
using System.Collections.Generic;
using System.Text;

namespace AutoAttendant.Models
{
    class UserTempSignUp
    {
        int _idLecture;
        string _email;
        string _password;

        public UserTempSignUp(int idLecture, string email, string password)
        {
            _idLecture = idLecture;
            _email = email;
            _password = password;
        }

        public int idLecture { get => _idLecture; set => _idLecture = value; }
        public string email { get => _email; set => _email = value; }
        public string password { get => _password; set => _password = value; }
    }
}

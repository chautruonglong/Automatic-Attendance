using System;
using System.Collections.Generic;
using System.Text;

namespace AutoAttendant.Models
{
    class UserTempSignUp
    {
        string _email;
        string _id_lecturer;
        //string _username;
        string _password;

        public UserTempSignUp(string email, string idLecture, string password)
        {
            _email = email;
            _id_lecturer = idLecture;
            //_username = username;
            _password = password;
        }

        public string email { get => _email; set => _email = value; }
        public string id_lecturer { get => _id_lecturer; set => _id_lecturer = value; }
        //public string username { get => _username; set => _username = value; }
        public string password { get => _password; set => _password = value; }
    }
}

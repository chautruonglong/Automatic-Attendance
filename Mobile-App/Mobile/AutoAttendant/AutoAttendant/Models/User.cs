using System;
using System.Collections.Generic;
using System.Text;

namespace AutoAttendant.Models
{
    public class User
    {
        //int _id; // sẽ bỏ cái ni
        int _idLecture;
        string _email;
        string _password;
        //string _id_lecturer;
        //string _authorization;

        public User(int idLecture, string email, string password)
        {
            _idLecture = idLecture;
            _email = email;
            _password = password;
        }

        //public int id { get => _id; set => _id = value; }
        public int idLecture { get => _idLecture; set => _idLecture = value; }
        public string email { get => _email; set => _email = value; }
        public string password { get => _password; set => _password = value; }
        //public string authorization { get => _authorization; set => _authorization = value; }
    }

    
}

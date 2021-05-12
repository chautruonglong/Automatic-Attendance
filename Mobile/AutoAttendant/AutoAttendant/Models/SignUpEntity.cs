using System;
using System.Collections.Generic;
using System.Text;

namespace AutoAttendant.Models
{
    class SignUpEntity
    {
        int _idLecture;
        string _email;
        string _name;
        string _password;

        public int IdLecture { get => _idLecture; set => _idLecture = value; }
        public string Email { get => _email; set => _email = value; }
        public string Name { get => _name; set => _name = value; }
        public string Password { get => _password; set => _password = value; }
    }
}

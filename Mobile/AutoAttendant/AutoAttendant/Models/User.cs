using System;
using System.Collections.Generic;
using System.Text;

namespace AutoAttendant.Models
{
    public class Tokens
    {
        string _access;

        public Tokens(string access)
        {
            this._access = access;
        }

        public string access { get => _access; set => _access = value; }
    }

    public class Data
    {
        string _email;
        string _id_lecturer;

        public string email { get => _email; set => _email = value; }
        public string id_lecturer { get => _id_lecturer; set => _id_lecturer = value; }
    }
    public class User
    {
        //int _id; // sẽ bỏ cái ni
        int _idLecture; // sẽ bỏ cái ni
        string _email;
        string _password;
        //string _id_lecturer;
        Tokens _tokens;
        //Data _data;

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
        public Tokens tokens { get => _tokens; set => _tokens = value; }
        //public string id_lecturer { get => _id_lecturer; set => _id_lecturer = value; }
        //public Data data { get => _data; set => _data = value; }
    }

    
}

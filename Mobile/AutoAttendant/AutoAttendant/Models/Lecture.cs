using System;
using System.Collections.Generic;
using System.Text;

namespace AutoAttendant.Models
{
    public class Lecture
    {
        private string _id_lecturer;
        private string _name; 
        private int _age;
        private string _phone;
        private string _faculty;




        public Lecture(string idLecture, string name, int age, string phone, string faculty)
        {
            this._id_lecturer = idLecture;
            this._name = name;
            this._age = age;
            this._phone = phone;
            this._faculty = faculty;
        }

        public string id_lecturer { get => _id_lecturer; set => _id_lecturer = value; }
        public string name { get => _name; set => _name = value; }
        public int age { get => _age; set => _age = value; }
        public string phone { get => _phone; set => _phone = value; }
        public string faculty { get => _faculty; set => _faculty = value; }
    }
}

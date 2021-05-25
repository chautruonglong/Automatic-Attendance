using System;
using System.Collections.Generic;
using System.Text;

namespace AutoAttendant.Models
{
    public class Lecture
    {
        private int _id;
        private string _name; 
        private string _age;
        private string _phone;
        private string _faculty;




        public Lecture(int id, string name)
        {
            this.id = id;
            this.name = name;
            //this._age = age;
            //this._phone = phone;
            //this._faculty = faculty;
        }

        public int id { get => _id; set => _id= value; }
        public string name { get => _name; set => _name = value; }
        public string age { get => _age; set => _age = value; }
        public string phone { get => _phone; set => _phone = value; }
        public string faculty { get => _faculty; set => _faculty = value; }
    }
}

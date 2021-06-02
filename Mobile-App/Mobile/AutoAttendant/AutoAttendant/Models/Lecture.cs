using System;
using System.Collections.Generic;
using System.Text;

namespace AutoAttendant.Models
{
    public class Lecture
    {
        private string _id;
        private string _name; 
        private string _phone;
        private string _faculty;




        public Lecture(string name, string phone, string faculty)
        {
            this.id = Data.Data.Instance.UserNui.lecturer_id;
            this.name = name;
            this._phone = phone;
            this._faculty = faculty;
        }

        public string id { get => _id; set => _id= value; }
        public string name { get => _name; set => _name = value; }
        public string phone { get => _phone; set => _phone = value; }
        public string faculty { get => _faculty; set => _faculty = value; }
    }
}

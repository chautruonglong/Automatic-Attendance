using System;
using System.Collections.Generic;
using System.Text;

namespace AutoAttendant.Models
{
    public class Lecture
    {
        private string id;
        private string name; 
        private string age;
        private string phone;
        private string faculty;

        public string Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        public string Age
        {
            get
            {
                return this.age;
            }
            set
            {
                this.age = value;
            }
        }

        public string Phone
        {
            get
            {
                return this.phone;
            }
            set
            {
                this.phone = value;
            }
        }

        public string Faculty
        {
            get
            {
                return this.faculty;
            }
            set
            {
                this.faculty = value;
            }
        }

        public Lecture()
        {

        }

        public Lecture(string id, string name, string age, string phone, string faculty)
        {
            this.Id = id;
            this.Name = name;
            this.Age = age;
            this.Phone = phone;
            this.Faculty = faculty;
        }
    }
}

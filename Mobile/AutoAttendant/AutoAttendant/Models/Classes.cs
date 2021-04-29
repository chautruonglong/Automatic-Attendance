using System;
using System.Collections.Generic;
using System.Text;

namespace AutoAttendant.Models
{
    public class Classes
    {
        string name;

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

        public Classes()
        {

        }

        public Classes(string name)
        {
            this.Name = name;
        }
    }
}

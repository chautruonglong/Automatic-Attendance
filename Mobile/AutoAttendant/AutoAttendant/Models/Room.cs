using System;
using System.Collections.Generic;
using System.Text;

namespace AutoAttendant.Models
{
    public class Room
    {
        private string id;
        private string name;

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

        public Room()
        {

        }

        public Room(string id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

    }

}

using System;
using System.Collections.Generic;
using System.Text;

namespace AutoAttendant.Models
{
    public class Room
    {
        private string id;
        private string name;
        private string state;
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

        public string State { get => state; set => state = value; }

        public Room(string id, string name,string state)
        {
            this.Id = id;
            this.Name = name;
            this.State = state;

        }

    }

}

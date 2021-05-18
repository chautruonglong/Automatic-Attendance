using System;
using System.Collections.Generic;
using System.Text;

namespace AutoAttendant.Models
{
    public class Room
    {
        private string _id;
        private string _name;
        private string _state;
        public string id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }

        public string name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        public string state { get => _state; set => _state = value; }

        public Room(string id, string name,string state)
        {
            this.id = id;
            this.name = name;
            this.state = state;

        }

    }

}

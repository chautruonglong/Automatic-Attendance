using System;
using System.Collections.Generic;
using System.Text;

namespace AutoAttendant.Models
{
    public class Room
    {
        private string _room_id;
        private string _state;
        public string room_id
        {
            get
            {
                return this._room_id;
            }
            set
            {
                this._room_id = value;
            }
        }



        public string state { get => _state; set => _state = value; }

        public Room(string id, string state)
        {
            this._room_id = id;
        }

    }

}

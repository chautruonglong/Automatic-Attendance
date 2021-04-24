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

        //public static List<Room> GetListRoom()
        //{
        //    return new List<Room>
        //    {
        //        new Room("1", "E101"),
        //        new Room("1", "E102"),
        //        new Room("1", "E103"),
        //        new Room("1", "E201"),
        //        new Room("1", "E202"),
        //        new Room("1", "E203"),
        //        new Room("1", "E301"),
        //        new Room("1", "E302"),
        //        new Room("1", "E303"),
        //        new Room("1", "E304"),
        //    };
        //}
    }

}

using System;
using System.Collections.Generic;
using System.Text;

namespace AutoAttendant.Models
{
    public class ListStd
    {
        string _subject_id;
        string _lecturer_id;
        string _room_id;
        string _name;
        string _time_slot;
        string _day;


        private List<StudentNui> _students =new List<StudentNui>();

        public ListStd()
        {
          
        }

        public string subject_id { get => _subject_id; set => _subject_id = value; }
        public string lecturer_id { get => _lecturer_id; set => _lecturer_id = value; }
        public string room_id { get => _room_id; set => _room_id = value; }
        public string name { get => _name; set => _name = value; }
        public string time_slot { get => _time_slot; set => _time_slot = value; }
        public string day { get => _day; set => _day = value; }
        public List<StudentNui> students { get => _students; set => _students = value; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace AutoAttendant.Models
{
    public class Subject
    {
        string _subject_id;
        string _id_lecturer;
        string _room_id;
        string _name;
        string _time_slot;
        string _day;
        private string _colorState = "#0E368B";
        private string _stateString;

        public Subject(string subject_id, string id_lecturer, string room_id, string name, string time_slot, string day)
        {
            _subject_id = subject_id;
            _id_lecturer = id_lecturer;
            _room_id = room_id;
            _name = name;
            _time_slot = time_slot;
            _day = day;
        }

        public string subject_id { get => _subject_id; set => _subject_id = value; }
        public string id_lecturer { get => _id_lecturer; set => _id_lecturer = value; }
        public string room_id { get => _room_id; set => _room_id = value; }
        public string name { get => _name; set => _name = value; }
        public string time_slot { get => _time_slot; set => _time_slot = value; }
        public string day { get => _day; set => _day = value; }
        public string colorState { get => _colorState; set => _colorState = value; }
        public string stateString { get => _stateString; set => _stateString = value; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace AutoAttendant.Models
{
    public class Subject
    {
        string _id_subject;
        string _id_lecturer;
        int _id_room;
        string _name;
        string _timeSlot;
        string _day;
        private string _colorState = "#0E368B";
        private string _stateString;

        public Subject(string id_subject, string id_lecture, int id_room, string name, string timeSlot, string day)
        {
            _id_subject = id_subject;
            _id_lecturer = id_lecture;
            _id_room = id_room;
            _name = name;
            _timeSlot = timeSlot;
            _day = day;
        }

        public string id_subject { get => _id_subject; set => _id_subject = value; }
        public string id_lecturer { get => _id_lecturer; set => _id_lecturer = value; }
        public int id_room { get => _id_room; set => _id_room = value; }
        public string name { get => _name; set => _name = value; }
        public string timeSlot { get => _timeSlot; set => _timeSlot = value; }
        public string day { get => _day; set => _day = value; }
        public string colorState { get => _colorState; set => _colorState = value; }
        public string stateString { get => _stateString; set => _stateString = value; }
    }
}

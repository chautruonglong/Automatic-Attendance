using System;
using System.Collections.Generic;
using System.Text;

namespace AutoAttendant.Models
{
    public class Schedule
    {
        private int _id;
        private string _idTeacher;
        private string _idRoom;
        private string _idSubject;
        private string _nameSubject;
        private string _timeSlot;
        private int _state;
        private DateTime _date;
        private string _colorState = "#0E368B";
        private string _stateString;

        public Schedule(int id, string idTeacher, string idRoom, string idSubject, string subject, string timeSlot, int state, DateTime date, string colorState, string stateString)
        {
            _id = id;
            _idTeacher = idTeacher;
            _idRoom = idRoom;
            _idSubject = idSubject;
            _nameSubject = subject;
            _timeSlot = timeSlot;
            _state = state;
            _date = date;
            _stateString = stateString;
        }

        public int id { get => _id; set => _id = value; }
        public string idTeacher { get => _idTeacher; set => _idTeacher = value; }
        public string idRoom { get => _idRoom; set => _idRoom = value; }
        public string idSubject { get => _idSubject; set => _idSubject = value; }
        public string nameSubject { get => _nameSubject; set => _nameSubject = value; }
        public string timeSlot { get => _timeSlot; set => _timeSlot = value; }
        public int state { get => _state; set => _state = value; }
        public DateTime date { get => _date; set => _date = value; }
        public string colorState { get => _colorState; set => _colorState = value; }
        public string stateString { get => _stateString; set => _stateString = value; }

        //public Schedule(int id, string idTeacher, string idRoom, string classes, string subject, string timeSlot, int state, string stateString, DateTime date)
        //{
        //    this.id = id;
        //    this.IdTeacher = idTeacher;
        //    this.IdRoom = idRoom;
        //    this.Classes = classes;
        //    this.Subject = subject;
        //    this.TimeSlot = timeSlot;
        //    this.State = state;
        //    this.StateString = stateString;
        //    this.Date = date;
        //}



    }

    
}

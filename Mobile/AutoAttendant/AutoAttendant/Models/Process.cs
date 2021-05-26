using System;
using System.Collections.Generic;
using System.Text;

namespace AutoAttendant.Models
{
    public class Process
    {
        int _id;
        string _id_subject;
        int _status;
        DateTime _date;
        string _time;

        public Process(string id_subject, int status, DateTime date, string time)
        {
            _id_subject = id_subject;
            _status = status;
            _date = date;
            _time = time;
        }
        public int id{ get => _id; set => _id = value; }
        public string id_subject { get => _id_subject; set => _id_subject = value; }
        public int status { get => _status; set => _status = value; }
        public DateTime date { get => _date; set => _date = value; }
        public string time { get => _time; set => _time = value; }
    }
}

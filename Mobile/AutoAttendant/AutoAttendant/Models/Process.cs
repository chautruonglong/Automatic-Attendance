using System;
using System.Collections.Generic;
using System.Text;

namespace AutoAttendant.Models
{
    public class Process
    {
        string _process_id;
        string _id_subject;
        bool _status;
        DateTime _date;
        string _time;

        public Process(string id_subject, bool status, DateTime date, string time)
        {
            _id_subject = id_subject;
            _status = status;
            _date = date;
            _time = time;
        }
        public string id_subject { get => _id_subject; set => _id_subject = value; }
        public bool status { get => _status; set => _status = value; }
        public DateTime date { get => _date; set => _date = value; }
        public string time { get => _time; set => _time = value; }
        public string process_id { get => _process_id; set => _process_id = value; }
    }
}

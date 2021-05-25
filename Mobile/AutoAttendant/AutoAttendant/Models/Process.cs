using System;
using System.Collections.Generic;
using System.Text;

namespace AutoAttendant.Models
{
    public class Process
    {
        int _id_process;
        string _id_subject;
        int _status;
        DateTime _date;
        string _time;

        public Process(int id_process, string id_subject, int status, DateTime date, string time)
        {
            _id_process = id_process;
            _id_subject = id_subject;
            _status = status;
            _date = date;
            _time = time;
        }
        public int id_process { get => _id_process; set => _id_process = value; }
        public string id_subject { get => _id_subject; set => _id_subject = value; }
        public int status { get => _status; set => _status = value; }
        public DateTime date { get => _date; set => _date = value; }
        public string time { get => _time; set => _time = value; }
    }
}

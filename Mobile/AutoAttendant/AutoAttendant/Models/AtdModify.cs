using System;
using System.Collections.Generic;
using System.Text;

namespace AutoAttendant.Models
{
    public class AtdModify
    {
        string _student_id;
        bool _state;

        public AtdModify(string student_id, bool state)
        {
            _student_id = student_id;
            _state = state;
        }

        public string student_id { get => _student_id; set => _student_id = value; }
        public bool state { get => _state; set => _state = value; }
    }
}

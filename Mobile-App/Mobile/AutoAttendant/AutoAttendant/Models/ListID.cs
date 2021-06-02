using System;
using System.Collections.Generic;
using System.Text;

namespace AutoAttendant.Models
{
    public class ListID
    {
        private string _subject_id;
        private List<string> _student_ids;

        public ListID(string subject_id, List<string> student_ids)
        {
            _subject_id = subject_id;
            _student_ids = student_ids;
        }

        public string subject_id { get => _subject_id; set => _subject_id = value; }
        public List<string> student_ids { get => _student_ids; set => _student_ids = value; }
    }
}

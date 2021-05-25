using System;
using System.Collections.Generic;
using System.Text;

namespace AutoAttendant.Models
{
    public class Attendance
    {
        private string _student_id;
        private bool _state;
        private string _face_3x4;
        private string _face_cam;

        public Attendance(string student_id, bool state, string face_3x4, string face_cam)
        {
            _student_id = student_id;
            _state = state;
            _face_3x4 = face_3x4;
            _face_cam = face_cam;
        }

        public string student_id { get => _student_id; set => _student_id = value; }
        public bool state { get => _state; set => _state = value; }
        public string face_3x4 { get => _face_3x4; set => _face_3x4 = value; }
        public string face_cam { get => _face_cam; set => _face_cam = value; }
    }

    
}

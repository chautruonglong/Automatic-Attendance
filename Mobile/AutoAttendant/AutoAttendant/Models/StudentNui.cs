using System;
using System.Collections.Generic;
using System.Text;

namespace AutoAttendant.Models
{
    public class StudentNui
    {
        string _student_id;
        string _name;
        string _phone;
        string _class_name;
        string _birth;
        string _img_3x4;
        bool _state = false;
        string _confidence = "";
        string _img_attendance;

        public StudentNui(string student_id, string name, string phone, string class_name,string birth, string img_3x4)
        {
            _student_id = student_id;
            _name = name;
            _phone = phone;
            _class_name = class_name;
            _birth = birth;
            _img_3x4 = img_3x4;
        }

        public string student_id { get => _student_id; set => _student_id = value; }
        public string name { get => _name; set => _name = value; }
        public string class_name { get => _class_name; set => _class_name = value; }
        public string phone { get => _phone; set => _phone = value; }
        public string birth { get => _birth; set => _birth = value; }
        public string img_3x4 { get => _img_3x4; set => _img_3x4 = value; }
        public bool state { get => _state; set => _state = value; }
        public string confidence { get => _confidence; set => _confidence = value; }
        public string img_attendance { get => _img_attendance; set => _img_attendance = value; }
    }
}

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
        string _birth;
        string _img_3x4;

        public StudentNui(string student_id, string name, string phone, string birth, string img_3x4)
        {
            _student_id = student_id;
            _name = name;
            _phone = phone;
            _birth = birth;
            _img_3x4 = img_3x4;
        }

        public string student_id { get => _student_id; set => _student_id = value; }
        public string name { get => _name; set => _name = value; }
        public string phone { get => _phone; set => _phone = value; }
        public string birth { get => _birth; set => _birth = value; }
        public string img_3x4 { get => img_3x4; set => img_3x4 = value; }
    }
}

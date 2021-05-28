using System;
using System.Collections.Generic;
using System.Text;

namespace AutoAttendant.Models
{
    public class AttendanceNui
    {
        string _type;
        string _id;
        string _confidence;
        string _img_face;

        public AttendanceNui(string type, string id, string confidence, string img_face)
        {
            _type = type;
            _id = id;
            _confidence = confidence;
            _img_face = img_face;
        }

        public string type { get => _type; set => _type = value; }
        public string id { get => _id; set => _id = value; }
        public string confidence { get => _confidence; set => _confidence = value; }
        public string img_face { get => _img_face; set => _img_face = value; }
    }
}

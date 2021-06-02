using System;
using System.Collections.Generic;
using System.Text;

namespace AutoAttendant.Models
{
    public class UserNui
    {
        string _lecturer_id;
        string _authorization;

        public UserNui(string lecturer_id, string authorization)
        {
            _lecturer_id = lecturer_id;
            _authorization = authorization;
        }

        public string lecturer_id { get => _lecturer_id; set => _lecturer_id = value; }
        public string authorization { get => _authorization; set => _authorization = value; }
    }
}

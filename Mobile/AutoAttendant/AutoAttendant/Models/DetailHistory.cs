using System;
using System.Collections.Generic;
using System.Text;

namespace AutoAttendant.Models
{
    public class DetailHistory
    {
        string _id;
        string _name;
        string _state;
        string _date1;
        string _date2;
        string _date3;
        string _date4;
        string _date5;

        public DetailHistory(string id, string name, string state, string date1, string date2, string date3, string date4, string date5)
        {
            Id = id;
            Name = name;
            State = state;
            Date1 = date1;
            Date2 = date2;
            Date3 = date3;
            Date4 = date4;
            Date5 = date5;
        }

        public string Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }
        public string State { get => _state; set => _state = value; }
        public string Date1 { get => _date1; set => _date1 = value; }
        public string Date2 { get => _date2; set => _date2 = value; }
        public string Date3 { get => _date3; set => _date3 = value; }
        public string Date4 { get => _date4; set => _date4 = value; }
        public string Date5 { get => _date5; set => _date5 = value; }
    }
}

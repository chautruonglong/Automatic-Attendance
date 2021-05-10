using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AutoAttendant.Models
{
    public class Classes
    {
        string name;
        string state;
        ObservableCollection<Student> StudentList = new ObservableCollection<Student>();

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        public ObservableCollection<Student> StudentList1 { get => StudentList; set => StudentList = value; }
        public string State { get => state; set => state = value; }

        public Classes()
        {

        }

        public Classes(string name, string state, ObservableCollection<Student> studentList)
        {
            this.Name = name;
            this.State = state;
            this.StudentList1 = studentList;
        }
    }
}

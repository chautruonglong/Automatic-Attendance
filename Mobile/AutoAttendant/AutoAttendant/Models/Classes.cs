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
        ObservableCollection<StudentNui> _StudentNuiList = new ObservableCollection<StudentNui>();

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
        public ObservableCollection<StudentNui> StudentNuiList { get => StudentNuiList; set => StudentNuiList = value; }

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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AutoAttendant.Models
{
    public class Classes
    {
        string name;
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

        public Classes()
        {

        }

        public Classes(string name, ObservableCollection<Student> studentList)
        {
            this.Name = name;
            this.StudentList1 = studentList;
        }
    }
}

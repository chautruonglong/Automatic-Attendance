using AutoAttendant.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AutoAttendant.ViewModel
{
    public class ListStudentViewModel
    {
        public ObservableCollection<Student> StudentCollection { get; set; }

        public Student RoomName { get; set; }

        public ListStudentViewModel()
        {
            StudentCollection = new ObservableCollection<Student>();
        }
    }
}

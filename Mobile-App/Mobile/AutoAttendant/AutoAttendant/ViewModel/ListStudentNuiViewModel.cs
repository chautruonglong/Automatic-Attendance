using AutoAttendant.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AutoAttendant.ViewModel
{
    public class ListStudentNuiViewModel
    {
        public ObservableCollection<StudentNui> StudentCollection { get; set; }

        public ListStudentNuiViewModel()
        {
            StudentCollection = new ObservableCollection<StudentNui>();
        }
    }
}

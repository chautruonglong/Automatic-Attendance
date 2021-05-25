using AutoAttendant.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AutoAttendant.ViewModel
{
    public class ListSubjectViewModel
    {
        public ObservableCollection<Subject> SubjectCollection { get; set; }

        public ListSubjectViewModel()
        {
            SubjectCollection = new ObservableCollection<Subject>();
        }
    }
}

using AutoAttendant.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AutoAttendant.ViewModel
{
    public class ListSubjectAllViewModel
    {
        public ObservableCollection<Subject> SubjectAllCollection { get; set; }

        public ListSubjectAllViewModel()
        {
            SubjectAllCollection = new ObservableCollection<Subject>();
        }
    }
}

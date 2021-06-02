using AutoAttendant.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AutoAttendant.ViewModel
{
    public class ListClassViewModel
    {
        public ObservableCollection<Classes> ClassCollection { get; set; }

        public Classes ClassName { get; set; }

        public ListClassViewModel()
        {
            ClassCollection = new ObservableCollection<Classes>();
        }
    }
}

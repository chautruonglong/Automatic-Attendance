using AutoAttendant.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AutoAttendant.ViewModel
{
    public class ListScheduleViewModel
    {
        public ObservableCollection<Schedule> ScheduleCollection { get; set; }

        public Schedule ScheduleName { get; set; }

        public ListScheduleViewModel()
        {
            ScheduleCollection = new ObservableCollection<Schedule>();
        }
    }
}

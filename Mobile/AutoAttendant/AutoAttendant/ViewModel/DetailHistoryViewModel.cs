using AutoAttendant.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AutoAttendant.ViewModel
{
    public class DetailHistoryViewModel
    {
        public ObservableCollection<DetailHistory> DetailHistoryCollection { get; set; }

        public DetailHistoryViewModel()
        {
            DetailHistoryCollection = new ObservableCollection<DetailHistory>();
        }
    }
}

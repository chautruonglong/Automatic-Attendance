using AutoAttendant.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AutoAttendant.ViewModel
{
    public class ListUnknownViewModel
    {
        public ObservableCollection<Room> RoomCollection { get; set; }


        public ListUnknownViewModel()
        {
            RoomCollection = new ObservableCollection<Room>();
        }
    }
}

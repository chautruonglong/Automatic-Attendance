using AutoAttendant.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AutoAttendant.ViewModel
{
    public class ListRoomViewModel
    {
        //public ICommand AddRoomCommand => new Command(AddRoom);
        public ObservableCollection<Room> RoomCollection { get; set; }

        public Room RoomName { get; set; }

        public ListRoomViewModel()
        {
            RoomCollection = new ObservableCollection<Room>();
        }

        //public void AddRoom()
        //{
        //    RoomCollection.Add(RoomName);
        //}
    }
}

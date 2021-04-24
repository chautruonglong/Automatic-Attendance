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
            //RoomCollection.Add(new Room("1", "F308"));
            //RoomCollection.Add(new Room("2", "F309"));
            //RoomCollection.Add(new Room("3", "F310"));
            //RoomCollection.Add(new Room("1", "F402"));
            //RoomCollection.Add(new Room("2", "F403"));
            //RoomCollection.Add(new Room("3", "F405"));
        }

        public void AddRoom()
        {
            RoomCollection.Add(RoomName);
        }
    }
}

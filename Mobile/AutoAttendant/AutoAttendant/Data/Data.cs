using AutoAttendant.Models;
using AutoAttendant.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoAttendant.Data
{
    class Data
    {
        private static Data _Instance;

        internal static Data Instance
        {
            get { if (_Instance == null) _Instance = new Data(); return _Instance; }
        }
        private User user;
        private Lecture lecture;

        public User User { get => user; set => user = value; }
        public Lecture Lecture { get => lecture; set => lecture = value; }

        private Data()
        {
           
            
        }

    }
}

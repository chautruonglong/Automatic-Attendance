using System;
using System.Collections.Generic;
using System.Text;

namespace AutoAttendant.Models
{
    public class Schedule
    {
        private string id;
        private string idTeacher;
        private string idRoom;
        private string classes;
        private string subject;
        private string timeSlot;
        private string state;

        public string Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }
        public string IdTeacher
        {
            get
            {
                return this.idTeacher;
            }
            set
            {
                this.idTeacher = value;
            }
        }
        public string IdRoom
        {
            get
            {
                return this.idRoom;
            }
            set
            {
                this.idRoom = value;
            }
        }
        public string Classes
        {
            get
            {
                return this.classes;
            }
            set
            {
                this.classes = value;
            }
        }
        public string Subject
        {
            get
            {
                return this.subject;
            }
            set
            {
                this.subject = value;
            }
        }

        public string TimeSlot
        {
            get
            {
                return this.timeSlot;
            }
            set
            {
                this.timeSlot = value;
            }
        }

        public string State
        {
            get
            {
                return this.state;
            }
            set
            {
                this.state = value;
            }
        }


        public Schedule(string id, string idTeacher, string idRoom, string classes, string subject, string timeSlot, string state)
        {
            this.Id = id;
            this.IdTeacher = idTeacher;
            this.IdRoom = idRoom;
            this.Classes = classes;
            this.Subject = subject;
            this.TimeSlot = timeSlot;
            this.State = state;
        }


    }

    
}

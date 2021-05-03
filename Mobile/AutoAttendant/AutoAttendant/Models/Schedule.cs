using System;
using System.Collections.Generic;
using System.Text;

namespace AutoAttendant.Models
{
    public class Schedule
    {
        private string classes;
        private string subject;
        private string timeSlot;

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

       
        public Schedule(string classes, string subject, string timeSlot)
        {
            this.Classes = classes;
            this.Subject = subject;
            this.TimeSlot = timeSlot;
        }


    }

    
}

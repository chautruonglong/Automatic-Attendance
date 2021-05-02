using System;
using System.Collections.Generic;
using System.Text;

namespace AutoAttendant.Models
{
    public class Schedule
    {
        private string subject;
        private string timeSlot;
        private string classs;


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

        public string Classs
        {
            get
            {
                return this.classs;
            }
            set
            {
                this.classs = value;
            }
        }

        public Schedule(string subject, string timeSlot, string classs)
        {
            this.Subject = subject;
            this.TimeSlot = timeSlot;
            this.Classs = classs;
        }


    }

    
}

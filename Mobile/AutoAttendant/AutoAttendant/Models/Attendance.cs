using System;
using System.Collections.Generic;
using System.Text;

namespace AutoAttendant.Models
{
    public class Attendance
    {
        private string idStudent;
        private string idSubject;
        private bool state;
        private string faceImg;
        private DateTime time;

        public string IdStudent
        {
            get
            {
                return this.idStudent;
            }
            set
            {
                this.idStudent = value;
            }
        }
        public string IdSubject
        {
            get
            {
                return this.idSubject;
            }
            set
            {
                this.idSubject = value;
            }
        }
        public bool State
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
        public string FaceImg
        {
            get
            {
                return this.faceImg;
            }
            set
            {
                this.faceImg = value;
            }
        }
        public DateTime Time
        {
            get
            {
                return this.time;
            }
            set
            {
                this.time = value;
            }
        }

        public Attendance()
        {

        }

        public Attendance(string idStudent, string idSubject, bool state, string faceImg, DateTime time)
        {
            this.IdStudent = idStudent;
            this.IdSubject = idSubject;
            this.State = state;
            this.FaceImg = faceImg;
            this.Time = time;
        }
    }

    
}

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace AutoAttendant.Models
{
    public class Student : INotifyPropertyChanged
    {
        private string id;
        private string name;
        private string classs;
        private string faculty;
        private string phone;
        private string imageUrl;

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

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
                OnPropertyChanged(nameof(Name));
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

        public string Faculty
        {
            get
            {
                return this.faculty;
            }
            set
            {
                this.faculty = value;
            }
        }

        public string Phone
        {
            get
            {
                return this.phone;
            }
            set
            {
                this.phone = value;
            }
        }

        public string ImageUrl
        {
            get
            {
                return this.imageUrl;
            }
            set
            {
                this.imageUrl = value;
            }
        }

        public Student()
        {

        }

        public Student(string id, string name, string classs, string faculty, string phone, string image)
        {
            this.Id = id;
            this.Name = name;
            this.Classs = classs;
            this.Faculty = faculty;
            this.Phone = phone;
            this.ImageUrl = image;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        //public static List<Student> GetListStudent()
        //{
        //    return new List<Student>()
        //    {

        //        new Student("102180171" , "Truong Long" , "18TCLC-DT2" , "102180171@sv1.dut.udn.vn" , "222222222" ,"long.png"),
        //        new Student("102180173" , "Chi Minh" , "18TCLC-DT2" , "102180173@sv1.dut.udn.vn" , "0000000" ,"minh.png"),
        //        new Student("102180180" , "Phuoc Quoc" , "18TCLC-DT2" , "102180173@sv1.dut.udn.vn" , "0000000" ,"quoc.png"),
        //        new Student("102180187" , "Khanh Toan" , "18TCLC-DT2" , "102180187@sv1.dut.udn.vn" , "00100100" ,"toan.png"),
        //        new Student("102180188" , "Minh Tri" , "18TCLC-DT2" , "102180188@sv1.dut.udn.vn" , "00100100" ,"tri.png"),
        //        new Student("102180191" , "Anh Tuan" , "18TCLC-DT2" , "102180191@sv1.dut.udn.vn" , "111111111" ,"tuan.png"),
        //        new Student("102180192" , "The Tue" , "18TCLC-DT2" , "102180192@sv1.dut.udn.vn" , "111111111" ,"tue.png"),
        //        new Student("102180194" , "Nguyen Vu" , "18TCLC-DT2" , "102180194@sv1.dut.udn.vn" , "3333333" ,"vu.png"),



        //    };
        //}


    }
}

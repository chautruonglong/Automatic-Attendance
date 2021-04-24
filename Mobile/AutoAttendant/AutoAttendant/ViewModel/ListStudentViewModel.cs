using AutoAttendant.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AutoAttendant.ViewModel
{
    public class ListStudentViewModel
    {
        public ObservableCollection<Student> StudentCollection { get; set; }

        public Student RoomName { get; set; }

        public ListStudentViewModel()
        {
            StudentCollection = new ObservableCollection<Student>();
            StudentCollection.Add(new Student("102180171", "Truong Long", "18TCLC-DT2", "102180171@sv1.dut.udn.vn", "222222222", "long.png"));
            StudentCollection.Add(new Student("102180173", "Chi Minh", "18TCLC-DT2", "102180173@sv1.dut.udn.vn", "0000000", "minh.png"));
            StudentCollection.Add(new Student("102180180", "Phuoc Quoc", "18TCLC-DT2", "102180173@sv1.dut.udn.vn", "0000000", "quoc.png"));
            StudentCollection.Add(new Student("102180187", "Khanh Toan", "18TCLC-DT2", "102180187@sv1.dut.udn.vn", "00100100", "toan.png"));
            StudentCollection.Add(new Student("102180188", "Minh Tri", "18TCLC-DT2", "102180188@sv1.dut.udn.vn", "00100100", "tri.png"));
            StudentCollection.Add(new Student("102180191", "Anh Tuan", "18TCLC-DT2", "102180191@sv1.dut.udn.vn", "111111111", "tuan.png"));
            StudentCollection.Add(new Student("102180194", "Nguyen Vu", "18TCLC-DT2", "102180194@sv1.dut.udn.vn", "3333333", "vu.png"));
        }
    }
}

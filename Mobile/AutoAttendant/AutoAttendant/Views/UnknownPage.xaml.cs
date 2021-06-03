using AutoAttendant.Models;
using AutoAttendant.Services;
using AutoAttendant.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoAttendant.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UnknownPage : ContentPage
    {
        ListUnknownViewModel luvm = new ListUnknownViewModel();
        List<StudentNui> notYetAtdListTemp = new List<StudentNui>();
        public UnknownPage(List<AttendanceNui> unknownList, List<StudentNui> notYetAtdList)
        {
            InitializeComponent();
            //this.BindingContext = new ListUnknownViewModel();
            StudentNui std1 = new StudentNui("102180191", "a", "12", "aa", "abc", "http://cb.dut.udn.vn/ImageSV/18/102180177.jpg");
            std1.state = true;
            StudentNui std2 = new StudentNui("102180173", "a", "12", "aa", "abc", "http://cb.dut.udn.vn/ImageSV/18/102180178.jpg");
            std2.state = true;
            notYetAtdList.Add(new StudentNui("3", "Tran Chi Minh", "12", "aa", "abc", "http://cb.dut.udn.vn/ImageSV/18/102180173.jpg"));
            notYetAtdList.Add(std1);
            notYetAtdList.Add(std2);
            notYetAtdList.Add(new StudentNui("4", "Chau Truong Long", "12", "aa", "abc", "http://cb.dut.udn.vn/ImageSV/18/102180174.jpg"));
            notYetAtdList.Add(new StudentNui("5", "Le Anh Tuan", "12", "aa", "abc", "http://cb.dut.udn.vn/ImageSV/18/102180175.jpg"));
            notYetAtdList.Add(new StudentNui("6", "Huynh Tran Khanh Toan", "12", "aa", "abc", "http://cb.dut.udn.vn/ImageSV/18/102180176.jpg"));
            notYetAtdList.Add(new StudentNui("7", "a", "12", "aa", "abc", "http://cb.dut.udn.vn/ImageSV/18/102180177.jpg"));
            notYetAtdList.Add(new StudentNui("8", "a", "12", "aa", "abc", "http://cb.dut.udn.vn/ImageSV/18/102180178.jpg"));


            unknownList.Add(new AttendanceNui("unknown", "1", "50", "http://cb.dut.udn.vn/ImageSV/18/102180173.jpg"));
            unknownList.Add(new AttendanceNui("unknown", "2", "60", "http://cb.dut.udn.vn/ImageSV/18/102180173.jpg"));
            unknownList.Add(new AttendanceNui("unknown", "3", "70", "http://cb.dut.udn.vn/ImageSV/18/102180173.jpg"));
            unknownList.Add(new AttendanceNui("unknown", "4", "80", "http://cb.dut.udn.vn/ImageSV/18/102180173.jpg"));
            unknownList.Add(new AttendanceNui("unknown", "1", "50", "http://cb.dut.udn.vn/ImageSV/18/102180173.jpg"));
            unknownList.Add(new AttendanceNui("unknown", "2", "60", "http://cb.dut.udn.vn/ImageSV/18/102180173.jpg"));
            unknownList.Add(new AttendanceNui("unknown", "3", "70", "http://cb.dut.udn.vn/ImageSV/18/102180173.jpg"));
            unknownList.Add(new AttendanceNui("unknown", "4", "80", "http://cb.dut.udn.vn/ImageSV/18/102180173.jpg"));
            notYetAtdListTemp = notYetAtdList;

            list_3x4.ItemsSource = notYetAtdListTemp;
            list_Unknown.ItemsSource = unknownList;
        }

        private void SaveUpdateAttendance(object sender, EventArgs e)
        {
            //Checkbox binding theo state cua notYetAtdList;
            //click checkbox se thay doi state cua std thuoc notYetAtdList;
            //sau khi bam save
            //duyet list notYetAtdList -> lay ra thong tin std co state = true
            //duyet single ListStudentPage.lsvm lay ra student.id == std.id

            foreach (StudentNui stdn in notYetAtdListTemp)
            {
                if (stdn.state == true)
                {
                    var update_std = ListStudentPage.lsnvm.StudentCollection.Single(r => r.student_id == stdn.student_id);
                    update_std.state = true;
                }
            }
            DisplayAlert("Notice", "Saved", "OK");
        }

        private void checkBoxClicked(object sender, EventArgs e)
        {

        }
    }
}
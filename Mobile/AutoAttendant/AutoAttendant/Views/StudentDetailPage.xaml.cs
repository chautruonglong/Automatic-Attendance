using AutoAttendant.Models;
using AutoAttendant.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoAttendant.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentDetailPage : ContentPage
    {
       
        public StudentDetailPage(StudentNui student, ListStudentViewModel lsvm )
        {
            InitializeComponent();
            ShowStudentInfo(student);
        }

        public void ShowStudentInfo(StudentNui student)
        {
            try
            {
                //Bo sung them student.ID
                lb_Name.Text = student.name;
                lb_Class.Text = student.birth;
                lb_Time.Text = student.phone;
                //Attendance atd = ListStudentPage.listAttendance.Single(r => r.student_id.Equals("102180171"));
                Avatar.Source = student.img_3x4;
                AvatarAttendance.Source = student.img_attendance;
                var x = student.state;
                if (x)
                {
                    btnAttendance.IsChecked = x;
                }
                else btnAbsent.IsChecked = !x;
            }
            catch(Exception ex)
            {
                DisplayAlert("ERROR", ex.Message, "OK");
            }



        }

        private void SaveAndBack(object sender, EventArgs e)
        {
            
            if(btnAttendance.IsChecked == true) 
            {
               var stu=ListStudentPage.lsvm.StudentCollection.Single(r => r.Name == lb_Name.Text); // chon student trong lsvm có name = lbName
               stu.State = true ;
            }
            if(btnAbsent.IsChecked == true)
            {
                var stu = ListStudentPage.lsvm.StudentCollection.Single(r => r.Name == lb_Name.Text);
                stu.State = false; ;
            }
            Navigation.PopAsync();
            
            
        }
    }
}
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
       
        public StudentDetailPage(Student student, ListStudentViewModel lsvm )
        {
            InitializeComponent();
            ShowStudentInfo(student);
            
        }

        public void ShowStudentInfo(Student student)
        {
            //Bo sung them student.ID
            lb_Name.Text = student.Name;
            lb_Class.Text = student.Classs;
            lb_Time.Text = student.Faculty;
            var x = student.State;
            if (x)
            {
                btnAttendance.IsChecked = x;
            }
            else btnAbsent.IsChecked = !x;
            
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
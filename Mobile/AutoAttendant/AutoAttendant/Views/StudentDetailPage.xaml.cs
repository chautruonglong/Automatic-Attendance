using AutoAttendant.Models;
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
        public StudentDetailPage(Student student )
        {
            InitializeComponent();
            ShowStudentInfo(student);

        }

        public void ShowStudentInfo(Student student)
        {
            lb_Name.Text = student.Name;
            lb_Class.Text = student.Classs;
            lb_Time.Text = student.Faculty;
        }
    }
}
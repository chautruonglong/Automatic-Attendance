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
    public partial class ListStudentPage : ContentPage
    {
        public ListStudentPage()
        {
            InitializeComponent();
            this.BindingContext = new ListStudentViewModel();
        }

        //public class ListStudentViewModel
        //{
        //    public List<Student> Collection { get; set; }

        //    public ListStudentViewModel()
        //    {
        //        Collection = Student.GetListStudent();
        //    }
        //}

        private void OnTapped(object sender, EventArgs e)
        {
            string message = string.Empty;
            Frame f = sender as Frame;
            var fContent = f.Content; // Lấy Content của Frame
            var myStacklayout = fContent.GetType(); // lấy kiểu của Content
            if (myStacklayout == typeof(StackLayout)) // check kiểu có phải Stack Layout ko
            {
                StackLayout fStacklayout = (StackLayout)fContent;
                var listChildren = fStacklayout.Children; // Lấy tập Children của StackLayout
                var firstLabel = listChildren[0];
                var secondLabel = listChildren[2];
                var thirdLabel = listChildren[3];

                //var isLabel = firstLabel.GetType(); // check kiểu của child đầu tiên
                if (firstLabel.GetType() == typeof(Label) && secondLabel.GetType() == typeof(Label) && thirdLabel.GetType() == typeof(Label))
                {
                    Label labelName = (Label)firstLabel;
                    Label labelClass = (Label)secondLabel;
                    Label labelTime = (Label)thirdLabel;
                    message = string.Format("Name: {0} \nClass: {1} \nTime: {2}", labelName.Text, labelClass.Text, labelTime.Text);
                    DisplayAlert("Notice", message, "OK");
                }

            }
        }

        private void OpenPicker(object sender, EventArgs e)
        {
            PickerSort.IsEnabled = true;
            PickerSort.Focus();
        }

        private void HandlePickerSort(object sender, EventArgs e)
        {
            var index = PickerSort.SelectedIndex;
            if(index != -1)
            {
                btnSortOption.Text = PickerSort.Items[index].ToString();
            }
        }
        private void ImportExcel(object sender, EventArgs e)
        {

        }

        private void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            string value = string.Empty;
            CheckBox cbStatus = sender as CheckBox;
            if (cbStatus.IsChecked == true)
            {
                btn_Excel.BackgroundColor = Color.FromHex("#246CFE");
            }
            else btn_Excel.BackgroundColor = Color.FromHex("#021135");
            //var check = StudentLayout.Children[1]; // lay ra Frame trong Student_Layout
            //if (check.GetType() == typeof(Frame))
            //{
            //    Frame fr_student = (Frame)check;
            //    fr_student.BackgroundColor = Color.FromHex("#246CFE");
            //}
        }
    }
}
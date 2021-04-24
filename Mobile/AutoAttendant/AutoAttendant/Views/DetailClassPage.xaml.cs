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
    public partial class DetailClassPage : ContentPage
    {
        public DetailClassPage()
        {
            InitializeComponent();
        }

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
    }
}
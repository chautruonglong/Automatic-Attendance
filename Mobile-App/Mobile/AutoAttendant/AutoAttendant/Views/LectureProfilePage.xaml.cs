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
    public partial class LectureProfilePage : ContentPage
    {
        public LectureProfilePage()
        {
            InitializeComponent();
            this.BindingContext = Data.Data.Instance.Lecture;
            SetUpLecturerInfo();
        }

        public void SetUpLecturerInfo()
        {
            Entry_email.Text = Data.Data.Instance.User.email;
            Entry_name.Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Data.Data.Instance.Lecture.name.ToLower());
            string avaText = "";
            Data.Data.Instance.Lecture.name.Split(' ').ToList().ForEach(i => avaText += i[0].ToString());
            Lecturer_Ava.Text = avaText;
            Lecturer_Ava.TextColor = Color.FromHex("#021135");
        }

        private void UpdateLecturerInfo(object sender, EventArgs e)
        {

        }
    }
}
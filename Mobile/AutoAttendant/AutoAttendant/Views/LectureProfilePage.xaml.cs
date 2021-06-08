using Acr.UserDialogs;
using AutoAttendant.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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

        [Obsolete]
        private async void UpdateLecturerInfo(object sender, EventArgs e)
        {
            try
            {
                var httpService = new HttpClient();
                var api_key = Data.Data.Instance.UserNui.authorization;
                httpService.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("authorization", api_key);
                var base_URL = HomePage.base_URL + "/lecturer/update/"+Data.Data.Instance.Lecture.id+ "/";
                Lecture lecturer = new Lecture(Entry_name.Text, Entry_phone.Text, Entry_faculty.Text);
                string jsonLecturer = JsonConvert.SerializeObject(lecturer);
                StringContent content = new StringContent(jsonLecturer, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpService.PutAsync(base_URL, content);
                if (response.IsSuccessStatusCode)
                {
                    UserDialogs.Instance.Toast("Update profile successfully!");
                }
                else
                {
                    await DisplayAlert("ERROR", "Fail to update your profile", "Try Again");
                }
            }
            catch(Exception ex)
            {
                await DisplayAlert("ERROR", "Lecturer Profile Page "+ ex.Message, "Try Again");
            }
            
        }

    }
}
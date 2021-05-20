using Acr.UserDialogs;
using AutoAttendant.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class SignUpLecturerPage : ContentPage
    {
        public SignUpLecturerPage()
        {
            InitializeComponent();
        }

        [Obsolete]
        public void GetLectureInfo()
        {
            var base_URL = HomePage.base_URL + "lecture?idTeacher=" + Data.Data.Instance.User.idLecture.ToString();
        }


        [Obsolete]
        private async void btnToHomePage(object sender, EventArgs e)
        {
            try
            {
                var httpService = new HttpClient();
                int idLecture = Convert.ToInt32(Entry_Id.Text);
                string name = Entry_name.Text;
                string age = Entry_age.Text;
                string phone = Entry_name.Text;
                string faculty = Entry_faculty.Text;

                

                //Declare Token
                var accessToken = Data.Data.Instance.User.token; //get token
                httpService.DefaultRequestHeaders.Accept.Clear();
                httpService.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                //Post new lecture
                var lecture = new Lecture(idLecture, name);
                string jsonLecture = JsonConvert.SerializeObject(lecture); // convert object => json
                StringContent contentLecture = new StringContent(jsonLecture, Encoding.UTF8, "application/json");
                var basePostLecture_URL = HomePage.base_URL + "lecture/";
                HttpResponseMessage responseLecture = await httpService.PostAsync(basePostLecture_URL, contentLecture);
                var result = await responseLecture.Content.ReadAsStringAsync();
                Data.Data.Instance.Lecture = JsonConvert.DeserializeObject<Lecture>(result); // gan cho static Lecture thong tin Lecture vua dang ki de dung` cho toan bo chtrinh

                //Get Lecture posted recently
                var baseGetLecture_URL = HomePage.base_URL + "lecture?idTeacher=" + Data.Data.Instance.User.idLecture.ToString();
                HttpResponseMessage response = await httpService.GetAsync(baseGetLecture_URL);
                var getLecture = await response.Content.ReadAsStringAsync();
                var listLecture = JsonConvert.DeserializeObject<ObservableCollection<Lecture>>(getLecture);
                //fake waiting
                UserDialogs.Instance.ShowLoading("Creating account...");
                await Task.Delay(1500);
                UserDialogs.Instance.HideLoading();
                

                if (responseLecture.IsSuccessStatusCode)
                {
                    UserDialogs.Instance.Toast("Your account was registered!");
                    //await Navigation.PushAsync(new HomePage(Data.Data.Instance.User)); 
                }
                else
                {
                    await DisplayAlert("ERROR", "Fail to register", "Try Again");
                }



            }
            catch (Exception)
            {
                await DisplayAlert("ERROR", "Fail to register", "Try Again");
            }
        }
    }
}
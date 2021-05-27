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
            //var base_URL = HomePage.base_URL + "lecture?idTeacher=" + Data.Data.Instance.User.idLecture.ToString();
            var base_URL = HomePage.base_URL + "lecture?idTeacher=" + Data.Data.Instance.Lecture.id.ToString();
        }


        [Obsolete]
        private async void btnToHomePage(object sender, EventArgs e)
        {
            try
            {
                var httpService = new HttpClient();
                string idLecture = Entry_Id.Text;
                string name = Entry_name.Text;
                int age = Convert.ToInt32(Entry_age.Text);
                string phone = Entry_phone.Text;
                string faculty = Entry_faculty.Text;



                //Declare Token
                var accessToken = ""; //get token
                httpService.DefaultRequestHeaders.Accept.Clear();
                httpService.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                //var api_key = User.api_key;
                //httpService.DefaultRequestHeaders.Accept.Add("authorization", api_key);
                //httpService.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", api_key);


                //HttpClient httpClient = new HttpClient();
                //HttpRequestMessage request = new HttpRequestMessage();
                //request.RequestUri = "Your_get_URI";
                //request.Method = HttpMethod.Get;
                //request.Headers.Add("api_key", "1234");
                //HttpResponseMessage response = await httpClient.SendAsync(request);
                //var responseString = await response.Content.ReadAsStringAsync();
                //var statusCode = response.StatusCode;

                //Post new lecture
                var lecture = new Lecture(Convert.ToInt32(idLecture), name);
                string jsonLecture = JsonConvert.SerializeObject(lecture); // convert object => json
                StringContent contentLecture = new StringContent(jsonLecture, Encoding.UTF8, "application/json");
                var basePostLecture_URL = @"http://42.114.97.127:8000/lecturer/detail/";
                HttpResponseMessage responseLecture = await httpService.PostAsync(basePostLecture_URL, contentLecture);
                var result = await responseLecture.Content.ReadAsStringAsync();
                Data.Data.Instance.Lecture = JsonConvert.DeserializeObject<Lecture>(result); // gan cho static Lecture thong tin Lecture vua dang ki de dung` cho toan bo chtrinh

                //Get Lecture posted recently
                //var baseGetLecture_URL = HomePage.base_URL + "lecture?idTeacher=" + Data.Data.Instance.User.idLecture.ToString();
                //var baseGetLecture_URL = HomePage.base_URL + "lecture?idTeacher=" + Data.Data.Instance.Lecture.id_lecturer.ToString();
                //HttpResponseMessage response = await httpService.GetAsync(baseGetLecture_URL);
                //var getLecture = await response.Content.ReadAsStringAsync();
                //var listLecture = JsonConvert.DeserializeObject<ObservableCollection<Lecture>>(getLecture);
                ////fake waiting
                //UserDialogs.Instance.ShowLoading("Creating account...");
                //await Task.Delay(1500);
                //UserDialogs.Instance.HideLoading();
                

                if (responseLecture.IsSuccessStatusCode)
                {
                    UserDialogs.Instance.Toast("Your account was registered!");
                    await Navigation.PushAsync(new HomePage()); 
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
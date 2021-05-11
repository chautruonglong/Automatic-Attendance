using AutoAttendant.Models;
using AutoAttendant.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using Acr.UserDialogs;
using System.Security.Cryptography;
using System.IO;
using Xamarin.Essentials;
using System.Net;
using AutoAttendant.ViewModel;

namespace AutoAttendant.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public static ListRoomViewModel _lrvm = new ListRoomViewModel(); 
        public static ListScheduleViewModel _lsvm = new ListScheduleViewModel();
        public static int checkCreateListSchedule = 0;
        public static Lecture _lecture = new Lecture();
        public LoginPage()
        {
            InitializeComponent();
        }
        
        private void ForgotPassword(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ForgotPasswordPage());
        }

        private void SignUp(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new SignUpPage());
            //await Application.Current.MainPage.Navigation.PushAsync(new SignUpPage());
        }

        //Tesstaaa
        [Obsolete]
        private async void LoginProcedure(object sender, EventArgs e)
        {
            try
            {
                User user = new User(Entry_user.Text, Entry_password.Text);
                if (user.CheckLogin())
                {
                    await Navigation.PushAsync(new HomePage());
                }
                else
                {
                    var httpService = new HttpService();
                    string full_url = "http://192.168.0.101:3000/posts/";   
                    var result = await httpService.SendAsync(full_url, HttpMethod.Get);

                    await DisplayAlert("JSON", result, "OK");

                    UserDialogs.Instance.ShowLoading("Please wait...");
                    await Task.Delay(2000);
                    UserDialogs.Instance.HideLoading();
                    await Navigation.PushAsync(new HomePage());
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("ERROR", ex.Message, "Try Again");
            }

            // Call API Login AutoAttendant here
            //try
            //{
            //    string base_URL = "http://IP:8000/user/login";
            //    var httpService = new HttpClient();
            //    string username = Entry_user.Text;
            //    string password = Entry_password.Text;
            //    var user = new User(username, password);

            //    string jsonData = JsonConvert.SerializeObject(user);
            //    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            //    HttpResponseMessage response = await httpService.PostAsync(base_URL, content); // post request to server and get respone
            //    var jsonResponse = await response.Content.ReadAsStringAsync();
            //    if () //check json response or response
            //    {
            //        await Navigation.PushAsync(new HomePage());
            //    }
            //}
            //catch (Exception)
            //{
            //    await DisplayAlert("ERROR", "Fail to register", "Try Again");
            //}

        }

        private void OpenApiEntry(object sender, EventArgs e)
        {
            Entry_Api.IsVisible = true;
        }
    }
}
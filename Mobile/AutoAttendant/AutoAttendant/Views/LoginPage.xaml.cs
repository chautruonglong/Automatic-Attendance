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
            //    var httpService = new HttpService();
            //    string user = Entry_user.Text;
            //    string password = Entry_password.Text;
            //    string full_url = "http://" + Entry_Api.Text + ":8000/login?" + "user=" + user + "&" + "password=" + password;

            //    var result = await httpService.SendAsync(url, HttpMethod.Post); 
            //    var account = JsonConvert.DeserializeObject<User>(result); // json response

            //    if (account.Username == user && account.Password == password)
            //    {
            //        await Navigation.PushAsync(new HomePage());
            //    }
            //}
            //catch (Exception)
            //{
            //    await DisplayAlert("ERROR", "User or Password is not correct", "Try Again");
            //}


        }

        private void OpenApiEntry(object sender, EventArgs e)
        {
            Entry_Api.IsVisible = true;
        }
    }
}
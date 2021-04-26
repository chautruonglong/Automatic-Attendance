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

namespace AutoAttendant.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        //protected override async void OnAppearing()
        //{
        //    //base.OnAppearing();
        //    var httpService = new HttpService();
        //    string api_key = "3XPeaCNzXoWSD3WMpU7f1rfYx8AvQmTj";
        //    string url = "https://api.giphy.com/v1/gifs/random";
        //    string full_url = url + "?api_key=" + api_key + "&tag=cat";
        //    var result = await httpService.SendAsync(full_url, HttpMethod.Get);
        //}

        private void ForgotPassword(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ForgotPasswordPage());
        }

        private void SignUp(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new SignUpPage());
            //await Application.Current.MainPage.Navigation.PushAsync(new SignUpPage());
        }

        [Obsolete]
        private async void LoginProcedure(object sender, EventArgs e)
        {
            //User user = new User(Entry_user.Text, Entry_password.Text);


            //if (user.CheckLogin())
            //{
            //    //DisplayAlert("Login", "Login Succesfully", "OK");
            //    Navigation.PushAsync(new HomePage());
            //}
            //else
            //{
            //    DisplayAlert("Login", "Fail", "Try Again");
            //}
            try
            {
                var httpService = new HttpService();
                string url = Entry_user.Text;
                string api_key = Entry_password.Text;
                string full_url = url + "?api_key=" + api_key + "&tag=cat";
                var result = await httpService.SendAsync(full_url, HttpMethod.Get);
                await DisplayAlert("JSON", result, "OK");
                UserDialogs.Instance.ShowLoading("Please wait...");
                await Task.Delay(2000);
                UserDialogs.Instance.HideLoading();
                await Navigation.PushAsync(new HomePage());
            //var json = JsonConvert.DeserializeObject<ObservableCollection<User>>(result);
            }
            catch(Exception)
            {
                await DisplayAlert("ERROR", "No Internet connected", "OK");
            }

            
        }
    }
}
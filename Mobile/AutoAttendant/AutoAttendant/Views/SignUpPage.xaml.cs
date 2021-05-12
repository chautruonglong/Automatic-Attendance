using Acr.UserDialogs;
using AutoAttendant.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoAttendant.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        string base_URL = "http://192.168.0.101:3000/account/";
        public SignUpPage()
        {
            InitializeComponent();
        }

        private void Login(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private async void btnSignUP(object sender, EventArgs e)
        {
            try
            {
                var httpService = new HttpClient();
                string email = Entry_email.Text;
                string password = Entry_password.Text;
                var user = new UserTemp(email, password);

                string jsonData = JsonConvert.SerializeObject(user); // convert object => json
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpService.PostAsync(base_URL, content); // post request to server and get respone

                //fake waiting
                UserDialogs.Instance.ShowLoading("Creating account...");
                await Task.Delay(2000);
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Toast("Your account was registered!");

                if (response.IsSuccessStatusCode)
                {
                    await Navigation.PopModalAsync();
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
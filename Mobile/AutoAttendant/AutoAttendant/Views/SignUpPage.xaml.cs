﻿using Acr.UserDialogs;
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
        public SignUpPage()
        {
            InitializeComponent();
        }

        private void Login(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        [Obsolete]
        private async void btnSignUP(object sender, EventArgs e)
        {
            try
            {
                var httpService = new HttpClient();
                int idLecture = Convert.ToInt32(Entry_Id.Text);
                string email = Entry_email.Text;
                string password = Entry_password.Text;
                string name = Entry_name.Text;

                var user = new UserTempSignUp(idLecture, email, password); // register
                var lecture = new Lecture(idLecture, name); //post lecture

                string jsonUser = JsonConvert.SerializeObject(user); // convert object => json
                StringContent contentUser = new StringContent(jsonUser, Encoding.UTF8, "application/json");
                var baseUser_URL = HomePage.base_URL + "register";
                HttpResponseMessage responseUser = await httpService.PostAsync(baseUser_URL, contentUser); // post request to server and get respone

                string jsonLecture = JsonConvert.SerializeObject(lecture); // convert object => json
                StringContent contentLecture = new StringContent(jsonLecture, Encoding.UTF8, "application/json");
                var baseLecture_URL = HomePage.base_URL + "lecture";
                HttpResponseMessage responseLecture = await httpService.PostAsync(baseLecture_URL, contentLecture);

                //fake waiting
                UserDialogs.Instance.ShowLoading("Creating account...");
                await Task.Delay(2000);
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Toast("Your account was registered!");

                if (responseUser.IsSuccessStatusCode && responseLecture.IsSuccessStatusCode)
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
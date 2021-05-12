﻿using AutoAttendant.Models;
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
        //public static ListRoomViewModel _lrvm = new ListRoomViewModel(); 
        //public static ListScheduleViewModel _lsvm = new ListScheduleViewModel();
        //public static int checkCreateListSchedule = 0; //avoid repeat schedule from ShowSchedule()
        //public static Lecture _lecture = new Lecture();
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
        }

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
                    var httpService = new HttpClient();
                    
                    string jsonData = JsonConvert.SerializeObject(user);
                    var base_URL = HomePage.base_URL + "login";
                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpService.PostAsync(base_URL, content);
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
        }

        private void OpenApiEntry(object sender, EventArgs e)
        {
            Entry_Api.IsVisible = true;
        }
    }
}
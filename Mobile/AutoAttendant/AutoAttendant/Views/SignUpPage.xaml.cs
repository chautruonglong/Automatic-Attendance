using Acr.UserDialogs;
using AutoAttendant.Models;
using AutoAttendant.Services;
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
        public static User SignUpResponse; // get data(email,id_lecturer) response after sign up
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
                ////string idLecture = Entry_Id.Text;
                var httpService = new HttpClient();
                string email = Entry_email.Text;
                string password = Entry_password.Text;
                string confirmPassword = Entry_confirm.Text;
                string name = Entry_name.Text;
                string phone = Entry_phone.Text;
                string faculty = Entry_degree.Text;

                if (password.Equals(confirmPassword))
                {
                    var userSignUp = new UserSignUp(name, phone, faculty, email, HashPW.HashPassword(password));
                    string jsonUserSignUp = JsonConvert.SerializeObject(userSignUp);
                    StringContent contentUserSignUp = new StringContent(jsonUserSignUp, Encoding.UTF8, "application/json");
                    var baseSignUp_URL = HomePage.base_URL +  "/account/signup/";
                    HttpResponseMessage responseSignUp = await httpService.PostAsync(baseSignUp_URL, contentUserSignUp);

                    //fake waiting
                    UserDialogs.Instance.ShowLoading("Creating account...");
                    await Task.Delay(1000);
                    UserDialogs.Instance.HideLoading();
                    UserDialogs.Instance.Toast("Your account was registered!");

                    if (responseSignUp.IsSuccessStatusCode)
                    {
                        await Navigation.PopModalAsync();
                    }
                    else
                    {
                        var result = await responseSignUp.Content.ReadAsStringAsync();
                        await DisplayAlert("ERROR", result, "Try Again");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("ERROR", "Fail in btnSignUp " + ex.Message , "Try Again");
            }
        }
    }
}
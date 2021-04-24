using AutoAttendant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoAttendant.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
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
            Navigation.PushAsync(new SignUpPage());
        }
        //Test Git
        //Test Git
        private void LoginProcedure(object sender, EventArgs e)
        {
            User user = new User(Entry_user.Text, Entry_password.Text);


            if (user.CheckLogin())
            {
                //DisplayAlert("Login", "Login Succesfully", "OK");
                Navigation.PushAsync(new HomePage());
            }
            else
            {
                DisplayAlert("Login", "Fail", "Try Again");
            }
        }
    }
}
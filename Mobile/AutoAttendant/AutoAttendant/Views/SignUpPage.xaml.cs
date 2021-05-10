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
        string base_URL = "http://IP:8000/user/register/";
        public SignUpPage()
        {
            InitializeComponent();
        }

        private void Login(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private void btnSignUP(object sender, EventArgs e)
        {
            //try
            //{
            //    var httpService = new HttpClient();
            //    string name = Entry_name.Text;
            //    string username = Entry_user.Text;
            //    string password = Entry_password.Text;  
            //    var user = new User(username, password);

            //    string jsonData = JsonConvert.SerializeObject(user);
            //    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            //    HttpResponseMessage response =  await httpService.PostAsync(base_URL, content); // post request to server and get respone
            //    var jsonResponse = await response.Content.ReadAsStringAsync();
            //    if () //check json response or response
            //    {
            //        await Navigation.PopModalAsync(); // back to login page
            //    }
            //}
            //catch (Exception)
            //{
            //    await DisplayAlert("ERROR", "Fail to register", "Try Again");
            //}
        }
    }
}
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
    public partial class SignUpLecturerPage : ContentPage
    {
        public SignUpLecturerPage()
        {
            InitializeComponent();
        }



        [Obsolete]
        private async void btnToHomePage(object sender, EventArgs e)
        {
            try
            {
                var httpService = new HttpClient();
                int idLecture = Convert.ToInt32(Entry_Id.Text);
                string name = Entry_name.Text;
                string age = Entry_age.Text;
                string phone = Entry_name.Text;
                string faculty = Entry_faculty.Text;


                var lecture = new Lecture(idLecture, name); //post lecture

                string jsonLecture = JsonConvert.SerializeObject(lecture); // convert object => json
                StringContent contentLecture = new StringContent(jsonLecture, Encoding.UTF8, "application/json");
                var baseLecture_URL = HomePage.base_URL + "lecture";
                HttpResponseMessage responseLecture = await httpService.PostAsync(baseLecture_URL, contentLecture);

                //Get Lecture Info
                try { }

                //fake waiting
                UserDialogs.Instance.ShowLoading("Creating account...");
                await Task.Delay(1500);
                UserDialogs.Instance.HideLoading();
                

                if (responseLecture.IsSuccessStatusCode)
                {
                    UserDialogs.Instance.Toast("Your account was registered!");
                    //await Navigation.PushAsync(new HomePage(Data.Data.Instance.User)); 
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
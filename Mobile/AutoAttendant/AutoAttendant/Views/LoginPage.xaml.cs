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
        public static int checkCreateListSchedule = 0; //avoid repeat schedule from ShowSchedule()
        //public static Lecture _lecture = new Lecture();
        public LoginPage()
        {
            InitializeComponent();
            GetSavedAccount();
        }
        #region Save Lastest Login
        public void SaveAccountLogined()
        {
            Preferences.Set("email", Entry_user.Text);
            Preferences.Set("password", Entry_password.Text);
        }

        public void GetSavedAccount()
        {
            Entry_user.Text = Preferences.Get("email", string.Empty);
            Entry_password.Text = Preferences.Get("password", string.Empty);
        }
        #endregion

        [Obsolete]
        private async void LoginProcedure(object sender, EventArgs e)
        {
            try
            {
                UserTemp userTemp = new UserTemp(Entry_user.Text, Entry_password.Text);
                var httpService = new HttpClient();

                string jsonData = JsonConvert.SerializeObject(userTemp); // dung` UserTemp để post login vì chỉ cần email vs password
                var base_URL = HomePage.base_URL + "/login";
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpService.PostAsync(base_URL, content);
                
                var result = await response.Content.ReadAsStringAsync();
                Data.Data.Instance.User = JsonConvert.DeserializeObject<User>(result); // dùng User để nhận json về vì có chứa thêm token, idLecture (static)
                User userMain = Data.Data.Instance.User;

                if (response.IsSuccessStatusCode)
                {
                    UserDialogs.Instance.ShowLoading("Please wait...");
                    await Task.Delay(2000);
                    UserDialogs.Instance.HideLoading();
                    SaveAccountLogined(); // save user and password for next time
                    await Navigation.PushAsync(new HomePage()); 
                    //await Navigation.PushAsync(new SignUpLecturerPage()); // sang page moi' de sign up thong tin lecturer
                }
                else await DisplayAlert("Error", "Login Fail", "Try Again");
            }
            catch (Exception ex)
            {
                await DisplayAlert("ERROR", ex.Message, "Try Again");
            }
        }

        private void ForgotPassword(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ForgotPasswordPage());
        }

        private void SignUp(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new SignUpPage());
        }

        private void OpenApiEntry(object sender, EventArgs e) 
        {
            Entry_Api.IsVisible = true;
        }

    }
}
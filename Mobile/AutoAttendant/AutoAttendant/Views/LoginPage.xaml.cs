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

        public async void HandleSchedule()
        {
            try
            {
                var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                { DevicePlatform.iOS, new[] {"com.microsoft.xlsx"} },
                { DevicePlatform.Android, new[] { "application/json" } },
            });
                var pickerResult = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = customFileType,
                    PickerTitle = "Pick an Excel file"
                });

                if (pickerResult != null)
                {
                    var resourcePath = pickerResult.FullPath.ToString();
                    using (StreamReader r = new StreamReader(resourcePath))
                    {
                        string json = r.ReadToEnd();
                        List<Schedule> items = JsonConvert.DeserializeObject<List<Schedule>>(json);
                        foreach (Schedule item in items)
                        {
                            string message = String.Format("Class: {0} \nSubject: {1} \nTime Slot: {2}", item.Classes, item.Subject, item.TimeSlot);
                            await DisplayAlert("Notice", message, "OK");
                        }
                    }
                }
            }
            catch(Exception)
            {
                await DisplayAlert("Notice", "Fail", "OK");
            }
        }
        //Tesstaaa
        [Obsolete]
        private async void LoginProcedure(object sender, EventArgs e)
        {
            // Test call API Gifphy
            //try
            //{
            //    var httpService = new HttpService();
            //    string url = Entry_user.Text;
            //    string api_key = Entry_password.Text;
            //    string full_url = url + "?api_key=" + api_key + "&tag=cat";
            //    var result = await httpService.SendAsync(full_url, HttpMethod.Get);

            //    await DisplayAlert("JSON", result, "OK");
            //    UserDialogs.Instance.ShowLoading("Please wait...");
            //    await Task.Delay(2000);
            //    UserDialogs.Instance.HideLoading();
            //    await Navigation.PushAsync(new HomePage());
            //    //var json = JsonConvert.DeserializeObject<ObservableCollection<User>>(result);
            //}
            //catch(Exception)
            //{
            //    await DisplayAlert("ERROR", "No Internet connected", "OK");
            //}
            try
            {
                User user = new User(Entry_user.Text, Entry_password.Text);
                if (user.CheckLogin())
                {
                    HandleSchedule();
                    await Navigation.PushAsync(new HomePage());
                }
                else
                {
                    var httpService = new HttpService();
                    string api_key = "3XPeaCNzXoWSD3WMpU7f1rfYx8AvQmTj";
                    string url = "https://api.giphy.com/v1/gifs/random";
                    string full_url = url + "?api_key=" + api_key + "&tag=cat";
                    var result = await httpService.SendAsync(full_url, HttpMethod.Get);
                    await DisplayAlert("JSON", result, "OK");

                    UserDialogs.Instance.ShowLoading("Please wait...");
                    await Task.Delay(2000);
                    UserDialogs.Instance.HideLoading();
                    await Navigation.PushAsync(new HomePage());
                }
            }
            catch (Exception)
            {
                await DisplayAlert("ERROR", "Login Fail", "Try Again");
            }

            // Call API Login AutoAttendant here
            //try
            //{
            //    var httpService = new HttpService();
            //    string user = Entry_user.Text;
            //    string password = Entry_password.Text;
            //    string url = " ";  // url of server

            //    var result = await httpService.SendAsync(url, HttpMethod.Get); 
            //    var user_json = JsonConvert.DeserializeObject<User>(result); // json response

            //    if (user_json.Username == user && user_json.Password == password)
            //    {
            //        await Navigation.PushAsync(new HomePage());
            //    }
            //}
            //catch (Exception)
            //{
            //    await DisplayAlert("ERROR", "User or Password is not correct", "Try Again");
            //}


        }
    }
}
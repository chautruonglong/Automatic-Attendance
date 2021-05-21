using AutoAttendant.Models;
using AutoAttendant.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoAttendant.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingPage : ContentPage
    {
        public SettingPage()
        {
            InitializeComponent();
        }

        [Obsolete]
        private async void SendRequest(object sender, EventArgs e)
        {
            try
            {
                var httpService = new HttpClient();
                
                string accessToken = Data.Data.Instance.User.tokens.access;
                var base_URL = HomePage.base_URL + "users";

                //Declare Token with request
                httpService.DefaultRequestHeaders.Accept.Clear();
                httpService.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                HttpResponseMessage response = await httpService.GetAsync(base_URL);
                var content = await response.Content.ReadAsStringAsync();
                var listUser = JsonConvert.DeserializeObject<ObservableCollection<User>>(content);

            }
            catch (Exception)
            {
                await DisplayAlert("Notice", "Fail", "OK");
            }
        }
    }
}
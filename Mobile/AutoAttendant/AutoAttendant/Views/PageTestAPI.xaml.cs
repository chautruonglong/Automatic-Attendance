using AutoAttendant.Models;
using AutoAttendant.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoAttendant.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageTestAPI : ContentPage
    {
        public PageTestAPI()
        {
            InitializeComponent();
        }

        public async Task<ObservableCollection<UserTemp>> GetUser()
        {
            var httpService = new HttpService();
            string full_url = Entry_url.Text;
            var result = await httpService.SendAsync(full_url, HttpMethod.Post);
            var listSchedule = JsonConvert.DeserializeObject<ObservableCollection<UserTemp>>(result);
            return listSchedule;
        }

        private void HandleAPI(object sender, EventArgs e)
        {
            try
            {
                ShowUser();
            }
            catch (Exception)
            {
                DisplayAlert("Error", "Can not get User", "OK");
            }
        }

        public async void ShowUser()
        {
            try
            {
                var listUser = new ObservableCollection<UserTemp>(await GetUser());
                foreach(UserTemp user in listUser)
                {
                    await DisplayAlert(user.Email, user.Username, user.Password);
                }

            }
            catch (Exception)
            {
                await DisplayAlert("Error", "Can not get User", "OK");
            }
        }
    }
}
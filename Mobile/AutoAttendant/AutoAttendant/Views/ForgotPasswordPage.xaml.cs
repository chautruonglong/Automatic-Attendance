using System;
using System.Collections.Generic;
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
    public partial class ForgotPasswordPage : ContentPage
    {
        public ForgotPasswordPage()
        {
            InitializeComponent();
        }

        private void ToVerificationPage(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PasswordVerificationPage());
        }

        [Obsolete]
        private async  void SendEmailToReset(object sender, EventArgs e)
        {
            var httpService = new HttpClient();
            string email = Entry_email.Text;
            var base_URL = HomePage.base_URL + "/account/password/reset/" + email + "/";
            var response = await httpService.GetAsync(base_URL);
            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Notice", "Check your email", "OK");
            }
            
        }
    }
}
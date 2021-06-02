using Acr.UserDialogs;
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
    public partial class ResetPasswordPage : ContentPage
    {
        public ResetPasswordPage()
        {
            InitializeComponent();
        }

        private async void ToLoginPage(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading("Please wait...");
            await Task.Delay(2000);
            UserDialogs.Instance.HideLoading();
            UserDialogs.Instance.Toast("Change password successfully");
            await Task.Delay(500);
            await Navigation.PushAsync(new LoginPage());
        }
    }
}
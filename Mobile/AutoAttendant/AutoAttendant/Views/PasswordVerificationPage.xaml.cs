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
    public partial class PasswordVerificationPage : ContentPage
    {
        public PasswordVerificationPage()
        {
            InitializeComponent();
        }

        private void ToResetPasswordPage(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ResetPasswordPage());
        }

        private void ResendCode(object sender, EventArgs e)
        {

        }
    }
}
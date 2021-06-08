using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoAttendant.Views.PopUp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopUpHistoryOptions
    {
        public PopUpHistoryOptions()
        {
            InitializeComponent();
        }

        public EventHandler<string> Action;

        private async void OpenPDF(object sender, EventArgs e)
        {
            Action?.Invoke(this, "View");
            await PopupNavigation.Instance.PopAsync();
        }

        private async void SendToEmail(object sender, EventArgs e)
        {
            Action?.Invoke(this, "Cancel");
            await PopupNavigation.Instance.PopAsync();
        }
    }
}
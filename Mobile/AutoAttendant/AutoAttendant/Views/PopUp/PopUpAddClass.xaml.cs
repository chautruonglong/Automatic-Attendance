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
    public partial class PopUpAddClass 
    {
        public PopUpAddClass()
        {
            InitializeComponent();
        }

        public EventHandler<string> Action;

        [Obsolete]
        private async void AddClass(object sender, EventArgs e)
        {
            string roomName = Entry_class.Text;
            Action?.Invoke(this, roomName);
            await PopupNavigation.Instance.PopAsync();
        }

        [Obsolete]
        private async void ClosePopUp(object sender, EventArgs e)
        {
            await PopupNavigation.PopAsync();
        }
    }
}
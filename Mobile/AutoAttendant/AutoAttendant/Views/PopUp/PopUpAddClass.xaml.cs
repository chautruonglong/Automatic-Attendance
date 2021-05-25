using AutoAttendant.Models;
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
        public static Subject subjectTemp;
        public PopUpAddClass(Subject subject)
        {
            InitializeComponent();
            this.BindingContext = subject;
            subjectTemp = subject;

        }

        

        public EventHandler<string> Action;

        [Obsolete]
        private async void JoinClass(object sender, EventArgs e)
        {
            //string roomName = Entry_class.Text;
            Action?.Invoke(this, "Join");
            await PopupNavigation.Instance.PopAsync();
        }

        [Obsolete]
        private async void ClosePopUp(object sender, EventArgs e)
        {
            await PopupNavigation.PopAsync();
        }

        private async void UpdateClass(object sender, EventArgs e)
        {
            //Handle data from PopUpUpdateSchedule


            // Call PopUp Update Class
            Action?.Invoke(this, "Update");
            await PopupNavigation.Instance.PopAsync();
        }

        private async void DeleteClass(object sender, EventArgs e)
        {
            Action?.Invoke(this, "Delete");
            await PopupNavigation.Instance.PopAsync();
        }

        private async void CancleClass(object sender, EventArgs e)
        {
            Action?.Invoke(this, "Cancel");
            await PopupNavigation.Instance.PopAsync();
        }
    }
}
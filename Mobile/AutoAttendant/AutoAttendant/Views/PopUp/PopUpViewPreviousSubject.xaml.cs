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
    public partial class PopUpViewPreviousSubject
    {
        public static Subject subjectTemp;
        public PopUpViewPreviousSubject(Subject subject)
        {
            InitializeComponent();
            this.BindingContext = subject;
            subjectTemp = subject;
        }

        public EventHandler<string> Action;
        private async void ViewClass(object sender, EventArgs e)
        {
            Action?.Invoke(this, "View");
            await PopupNavigation.Instance.PopAsync();
        }

        private async void CancleClass(object sender, EventArgs e)
        {
            Action?.Invoke(this, "Cancel");
            await PopupNavigation.Instance.PopAsync();
        }
    }
}
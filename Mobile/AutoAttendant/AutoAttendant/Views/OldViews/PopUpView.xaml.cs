using AutoAttendant.Models;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
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
    public partial class PopUpView
    {
        public PopUpView()
        {
            InitializeComponent();
        }

        [Obsolete]
        public EventHandler<string> Action;

        [Obsolete]
        private async void AddRoom(object sender, EventArgs e)
        {
            string roomName = Entry_room.Text;
            Room room = new Room("1", "x202");
            var jsonRoom = JsonConvert.SerializeObject(room);
            Action?.Invoke(this, jsonRoom);
            await PopupNavigation.Instance.PopAsync();
        }

        [Obsolete]
        private async void ClosePopUp(object sender, EventArgs e)
        {
            await PopupNavigation.PopAsync();
        }
    }
}
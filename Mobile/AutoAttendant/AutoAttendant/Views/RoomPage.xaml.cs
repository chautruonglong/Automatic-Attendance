using AutoAttendant.Models;
using AutoAttendant.ViewModel;
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
    public partial class RoomPage : ContentPage
    {
        ListRoomViewModel lrvm = new ListRoomViewModel();
        public RoomPage()
        {
            InitializeComponent();
            HandleDatePicker();
            this.BindingContext = new ListRoomViewModel();
        }


        private void RoomClick(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ClassPage());
        }

        private void HandleDatePicker()
        {
            string message = string.Empty;
            var datePicker = new DatePicker
            {
                ClassId = "MyDatePicker",
                Date = DateTime.Now,
                IsVisible = false,
                IsEnabled = false
                
            };
            DatePickerLayout.Children.Add(datePicker);

            btnDatePicker.Clicked += (object sender, EventArgs e) =>
            {
                
                IsEnabled = true;
                datePicker.Focus();
            };
            
            datePicker.DateSelected += (object sender1, DateChangedEventArgs e) =>
            {
                message = datePicker.Date.ToString();
                DisplayAlert("Notice", message, "OK");
            };
        }

        [Obsolete]
        private async void AddRoom(object sender, EventArgs e)
        {
            await PopupNavigation.PopAsync();

        }

        //public class ListRoomViewModel
        //{
        //    public ObservableCollection<Room> RoomCollection { get; set; }

        //    public ListRoomViewModel()
        //    {
        //        RoomCollection = new ObservableCollection<Room>(Room.GetListRoom());
        //    }
        //}


        [Obsolete]
        private async void ShowPopUpAddRoom(object sender, EventArgs e) // Show Popup and handle data from popup
        {
            string roomName = String.Empty;
            string message = String.Empty;
            var page = new PopUpView();
            page.Action += async (sender1, stringparameter) =>
            {
                
                if(stringparameter != null)
                {
                    roomName = stringparameter; // get data tu` PopUp
                    roomName = roomName.Replace(" ", "");
                    roomName = roomName.ToUpper();
                    message = roomName + " was added successfully";

                    Room room = new Room("1", roomName);
                    lrvm.RoomCollection.Add(room);
                    this.BindingContext = lrvm;


                    await DisplayAlert("Notice", message, "OK");

                }
                else
                {
                    await DisplayAlert("Notice", "Invalid Syntax" , "OK");
                    return;
                }
            };

            page.Disappearing += (c, d) =>
            {
                if (roomName != null)
                {
                    

                }
                
            };

            await PopupNavigation.Instance.PushAsync(page);
            //PopupNavigation.PushAsync(new PopUpView());
        }
    }
}
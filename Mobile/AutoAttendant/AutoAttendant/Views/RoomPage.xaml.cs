﻿using Acr.UserDialogs;
using AutoAttendant.Models;
using AutoAttendant.Services;
using AutoAttendant.ViewModel;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
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

        [Obsolete]
        public RoomPage()
        {
            InitializeComponent();
            HandleDatePicker();
            this.BindingContext = new ListRoomViewModel();
            ShowRoom();
        }
        protected override void OnAppearing() // goị trước khi screen page này xuất hiện
        {
            ReLoadRoomList();
            base.OnAppearing();
        }

        [Obsolete]
        public void ReLoadRoomList()
        {
            //SetColorById();
            //lsvm.ScheduleCollection = HomePage._lsvm.ScheduleCollection;
            if (lrvm.RoomCollection.Count > 0)
            {

                this.BindingContext = new ListRoomViewModel();
                this.BindingContext = lrvm;
                //SetColorById();

            }
        }

        private void RoomClick(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new ClassPage());
        }

        private void HandleDatePicker()
        {
            //string message = string.Empty;
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
                string day = datePicker.Date.DayOfWeek.ToString();
                string date = datePicker.Date.ToString().Substring(0, 9);

                lb_date.Text = "Rooms on " + day + " " + date;
            };
        }

        [Obsolete]
        private async void AddRoom(object sender, EventArgs e)
        {
            await PopupNavigation.PopAsync();

        }

        [Obsolete]
        public  async Task<ObservableCollection<Room>> HandleRoom()
        {
            try
            {
                var httpService = new HttpService();
                var base_URL = HomePage.base_URL + "room";
                //string full_url = "http://192.168.0.101:3000/room/";
                var result = await httpService.SendAsync(base_URL, HttpMethod.Get);
                //WebClient wc = new WebClient();
                //var result = wc.DownloadString(full_url);
                var listRoom = JsonConvert.DeserializeObject<ObservableCollection<Room>>(result);
                return listRoom;
            }
            catch (Exception)
            {
                await DisplayAlert("Notice", "Fail", "OK");
                return null;
            }
        }

        [Obsolete]
        public async void ShowRoom()
        {
            try
            {
                HomePage._lrvm.RoomCollection.Clear();
                var listRoom = new ObservableCollection<Room>(await HandleRoom());
                foreach (Room room in listRoom)
                {
                    //lrvm.RoomCollection.Add(room);
                    HomePage._lrvm.RoomCollection.Add(room);
                }
                lrvm.RoomCollection = HomePage._lrvm.RoomCollection;
                this.BindingContext = lrvm;
            }
            catch (Exception)
            {
                await DisplayAlert("Error", "Can not get Room", "OK");
            }

        }

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
                    //message = roomName + " was added successfully";

                    Room room = new Room("1", roomName,"Available");
                    lrvm.RoomCollection.Add(room);
                   // HomePage._lrvm.RoomCollection.Add
                    this.BindingContext = lrvm;

                    UserDialogs.Instance.Toast("Room " + roomName + " was added");

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

        private void DeleteRoom(object sender, EventArgs e)
        {
            Image img = sender as Image;
            var stackLayout = img.Parent;
            var checkStack = stackLayout.GetType();
            if (checkStack == typeof(StackLayout))
            {
                StackLayout container = (StackLayout)stackLayout;
                var listChild = container.Children;

                var lb_room = listChild[0].GetType();
                if (lb_room == typeof(Label))
                {
                    Label lb = (Label)listChild[0];

                    var itemToRemove = lrvm.RoomCollection.Single(r => r.Name == lb.Text);
                    lrvm.RoomCollection.Remove(itemToRemove);
                }
                else
                {
                    DisplayAlert("Fail", "Fail", "Try Again");
                }

            }
        }
    }
}
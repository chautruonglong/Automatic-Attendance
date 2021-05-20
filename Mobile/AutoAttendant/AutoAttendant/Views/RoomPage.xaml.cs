using Acr.UserDialogs;
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
        private static ObservableCollection<Room> listRoomTemp = new ObservableCollection<Room>(); // a copy from lrvm.RoomCollection

        [Obsolete]
        public RoomPage()
        {
            InitializeComponent();
            HandleDatePicker();
            this.BindingContext = new ListRoomViewModel();
            ShowRoom();
            listRoomTemp = lrvm.RoomCollection;
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

        public List<string> GetRoomNames()
        {
            List<string> roomNames = new List<string>();
            foreach(Room room in lrvm.RoomCollection)
            {
                roomNames.Add(room.name);
            }
            return roomNames;
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

                // order Room by name
                listRoom= new ObservableCollection<Room>(listRoom.OrderBy(r => r.name));
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
                    //this.BindingContext = lrvm; // chu y

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

                    var itemToRemove = lrvm.RoomCollection.Single(r => r.name == lb.Text);
                    lrvm.RoomCollection.Remove(itemToRemove);
                }
                else
                {
                    DisplayAlert("Notice", "Fail", "Try Again");
                }

            }
        }

        private void SearchRoom_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var listRoomNames = GetRoomNames();  //list string room names
                

                var searchRoom = e.NewTextValue; //text from search bar
                if (string.IsNullOrWhiteSpace(searchRoom))
                {
                    searchRoom = string.Empty;
                }

                searchRoom = searchRoom.ToLowerInvariant();
                var filterdRooms = listRoomNames.Where(r => r.ToLowerInvariant().Contains(searchRoom)).ToList(); // rooms have name contains text in search bar 
                if (string.IsNullOrWhiteSpace(searchRoom))
                {
                    filterdRooms = listRoomNames;
                }

                foreach (var value in listRoomNames)
                {
                    if (!filterdRooms.Contains(value))  
                    {
                        var roomToRemove = lrvm.RoomCollection.Where(r => r.name == value).ToList();
                        foreach(var item in roomToRemove)
                        {
                            lrvm.RoomCollection.Remove(item); //remove rooms that dont have name in filterdRoom
                        }
                    }
                    //else if(!lrvm.RoomCollection.Intersect(lrvm.RoomCollection.Where(r => r.name == value).ToList()).Any()) //check cho else
                    //{
                    //    var listRoomRefresh = new List<String>();
                    //    foreach(var room in lrvm.RoomCollection)
                    //    {
                    //        listRoomRefresh.Add(room.name);
                    //    }
                    //    if (!listRoomRefresh.Contains(value))
                    //    {
                    //        var roomToAdd = listRoomTemp.Single(r => r.name == value);
                    //        lrvm.RoomCollection.Add(roomToAdd);
                    //        this.BindingContext = lrvm;
                    //    }
                    //}
                }
            }
            catch (Exception)
            {
                DisplayAlert("Fail", "Fail", "Try Again");
            }
            
        }
    }
}
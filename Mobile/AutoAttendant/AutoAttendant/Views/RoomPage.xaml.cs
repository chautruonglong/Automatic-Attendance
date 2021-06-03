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
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoAttendant.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RoomPage : ContentPage
    {
        public static ListRoomViewModel lrvm = new ListRoomViewModel();
        private static List<Room> listRoomTemp = new List<Room>(); // a copy from lrvm.RoomCollection

        [Obsolete]
        public RoomPage()
        {
            InitializeComponent();
            //HandleDatePicker();
            this.BindingContext = new ListRoomViewModel();
            ShowRoom();
        }

        [Obsolete]
        protected override void OnAppearing() // goị trước khi screen page này xuất hiện
        {
            ReLoadRoomList();
            base.OnAppearing();
        }

        [Obsolete]
        public void ReLoadRoomList()
        {
            if (lrvm.RoomCollection.Count > 0)
            {
                this.BindingContext = new ListRoomViewModel();
                this.BindingContext = lrvm;
            }
        }

        [Obsolete]
        public  async Task<ObservableCollection<Room>> HandleRoom() //Get all room
        {
            try
            {
                var httpService = new HttpClient();
                var api_key = Data.Data.Instance.UserNui.authorization;
                httpService.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("authorization", api_key);
                var base_URL = HomePage.base_URL + "/room/list/";
                var result = await httpService.GetAsync(base_URL);
                var responseRoom = await result.Content.ReadAsStringAsync();
                var listRoom = JsonConvert.DeserializeObject<ObservableCollection<Room>>(responseRoom);

                // order Room by name
                listRoom = new ObservableCollection<Room>(listRoom.OrderBy(r => r.room_id));
                return listRoom;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Notice", ex.Message, "OK");
                return null;
            }
        }

        [Obsolete]
        public async Task<List<string>> Handle_TimeSlot_Of_a_Room(string idRoom) //get all time slot in a room 
        {
            try
            {
                var httpService = new HttpClient();
                var api_key = Data.Data.Instance.UserNui.authorization;
                httpService.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("authorization", api_key);
                var base_URL = HomePage.base_URL + "/subject/time_slot/list/" + idRoom + "/" + DateTime.Now.DayOfWeek.ToString() + "/";
                var result = await httpService.GetAsync(base_URL);
                var responseTimeSlot = await result.Content.ReadAsStringAsync();
                
                var listTimeSlot_a_room = JsonConvert.DeserializeObject<List<string>>(responseTimeSlot);
                // order list subject by time slot
               // listTimeSlot_a_room = new ObservableCollection<string>(listSubject.OrderBy(r => r.time_slot));

                //List<string> listTimeSlot_a_room = new List<string>();
                //foreach(Subject subject in listSubject)
                //{
                //    listTimeSlot_a_room.Add(subject.time_slot);
                //}

                return listTimeSlot_a_room;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
                return null;
            }
        }
 
        public string SetState_For_EachRoom(List<string> listTimeSlot_a_room)
        {
            foreach (string timeSlot in listTimeSlot_a_room)
            {
                TimeSpan TimeBegin = TimeSpan.Parse(timeSlot.Substring(0, 5));
                TimeSpan TimeEnd = TimeSpan.Parse(timeSlot.Substring(6));
                TimeSpan timeNow = DateTime.Now.TimeOfDay;
                if (timeNow > TimeBegin && timeNow < TimeEnd)
                {
                    return "Using: " + TimeBegin + "-" + TimeEnd;
                }
            }
            return "Available";
        }

        [Obsolete]
        public async void ShowRoom()
        {
            try
            {
                lrvm.RoomCollection.Clear();

                //HandleStateRoom(HomePage._lsjvm.SubjectCollection);
                var listRoom = new ObservableCollection<Room>(await HandleRoom());
                foreach (Room room in listRoom)
                {
                    List<string> listTimeSlot = await Handle_TimeSlot_Of_a_Room(room.room_id);
                    room.state = SetState_For_EachRoom(listTimeSlot);
                    lrvm.RoomCollection.Add(room);
                }
                this.BindingContext = lrvm;
                listRoomTemp = lrvm.RoomCollection.ToList();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        #region Search Click
        private void SearchRoom_TextChanged(object sender, TextChangedEventArgs e)
        {
        //    try
        //    {
        //        var listRoomAll = listRoomTemp;

        //        var searchRoom = e.NewTextValue; //text from search bar
        //        if (string.IsNullOrWhiteSpace(searchRoom))
        //        {
        //            searchRoom = string.Empty;
        //        }

        //        searchRoom = searchRoom.ToLowerInvariant();
        //        var filterdRooms = listRoomAll.Where(r => r.name.ToLowerInvariant().Contains(searchRoom)).ToList(); // rooms have name contains text in search bar 
        //        if (string.IsNullOrWhiteSpace(searchRoom))
        //        {
        //            filterdRooms = listRoomAll.ToList();
        //        }

        //        foreach (var room in listRoomAll)
        //        {
        //            if (!filterdRooms.Contains(room))
        //            {
        //                lrvm.RoomCollection.Remove(room); //remove rooms that dont have name in filterdRoom
        //            }
        //            else if (!lrvm.RoomCollection.Contains(room))
        //            {
        //                lrvm.RoomCollection.Add(room);
        //                lrvm.RoomCollection.OrderBy(r => r.name);
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        DisplayAlert("Fail", "Fail", "Try Again");
        //    }
        }
        #endregion

        #region Temp
        //private void HandleDatePicker()
        //{
        //    //string message = string.Empty;
        //    var datePicker = new DatePicker
        //    {
        //        ClassId = "MyDatePicker",
        //        Date = DateTime.Now,
        //        IsVisible = false,
        //        IsEnabled = false

        //    };
        //    DatePickerLayout.Children.Add(datePicker);

        //    btnDatePicker.Clicked += (object sender, EventArgs e) =>
        //    {

        //        IsEnabled = true;
        //        datePicker.Focus();
        //    };

        //    datePicker.DateSelected += (object sender1, DateChangedEventArgs e) =>
        //    {
        //        string day = datePicker.Date.DayOfWeek.ToString();
        //        string date = datePicker.Date.ToString().Substring(0, 9);

        //        lb_date.Text = "Rooms on " + day + " " + date;
        //    };
        //}

        //[Obsolete]
        //private async void AddRoom(object sender, EventArgs e)
        //{
        //    await PopupNavigation.PopAsync();
        //}

        //[Obsolete]
        //private async void ShowPopUpAddRoom(object sender, EventArgs e) // Show Popup and handle data from popup
        //{
        //    string roomName = String.Empty;
        //    string message = String.Empty;
        //    var page = new PopUpView();
        //    page.Action += async (sender1, stringparameter) =>
        //    {

        //        if (stringparameter != null)
        //        {
        //            roomName = stringparameter; // get data tu` PopUp
        //            roomName = roomName.Replace(" ", "");
        //            roomName = roomName.ToUpper();
        //            //message = roomName + " was added successfully";

        //            Room room = new Room("1", roomName, "Available");
        //            lrvm.RoomCollection.Add(room);
        //            // HomePage._lrvm.RoomCollection.Add
        //            //this.BindingContext = lrvm; // chu y

        //            UserDialogs.Instance.Toast("Room " + roomName + " was added");

        //        }
        //        else
        //        {
        //            await DisplayAlert("Notice", "Invalid Syntax", "OK");
        //            return;
        //        }
        //    };

        //    page.Disappearing += (c, d) =>
        //    {
        //        if (roomName != null)
        //        {

        //        }

        //    };

        //    await PopupNavigation.Instance.PushAsync(page);
        //    //PopupNavigation.PushAsync(new PopUpView());
        //}

        //private void DeleteRoom(object sender, EventArgs e)
        //{
        //    Image img = sender as Image;
        //    var stackLayout = img.Parent;
        //    var checkStack = stackLayout.GetType();
        //    if (checkStack == typeof(StackLayout))
        //    {
        //        StackLayout container = (StackLayout)stackLayout;
        //        var listChild = container.Children;

        //        var lb_room = listChild[0].GetType();
        //        if (lb_room == typeof(Label))
        //        {
        //            Label lb = (Label)listChild[0];

        //            var itemToRemove = lrvm.RoomCollection.Single(r => r.name == lb.Text);
        //            lrvm.RoomCollection.Remove(itemToRemove);
        //        }
        //        else
        //        {
        //            DisplayAlert("Notice", "Fail", "Try Again");
        //        }

        //    }
        //}
        #endregion
    }
}
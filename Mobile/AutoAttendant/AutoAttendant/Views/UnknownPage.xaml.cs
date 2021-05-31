using AutoAttendant.Models;
using AutoAttendant.Services;
using AutoAttendant.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoAttendant.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UnknownPage : ContentPage
    {
        ListUnknownViewModel luvm = new ListUnknownViewModel();
        public UnknownPage()
        {
            InitializeComponent();
            this.BindingContext = new ListUnknownViewModel();
            ObservableCollection<Room> list2 = new ObservableCollection<Room>();
            list2.Add(new Room("https://www.seekpng.com/png/small/67-679395_youtube-play-png-youtube-2018-logo-png.png", "xyz"));
            list2.Add(new Room("https://tuoitreit.vn/data/avatars/o/88/88344.jpg?1566350734", "xyz"));
            list2.Add(new Room("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ7E3YMQl6YDwv3ccD11ksoB8kHJ3Zd0E2bSZy9whl6nM9Gwy8LjzvUFv-4jHbfy0whOPM&usqp=CAU", "xyz"));
            list2.Add(new Room("https://www.seekpng.com/png/small/67-679395_youtube-play-png-youtube-2018-logo-png.png", "xyz"));
            list2.Add(new Room("https://tuoitreit.vn/data/avatars/o/88/88344.jpg?1566350734", "xyz"));
            list2.Add(new Room("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ7E3YMQl6YDwv3ccD11ksoB8kHJ3Zd0E2bSZy9whl6nM9Gwy8LjzvUFv-4jHbfy0whOPM&usqp=CAU", "xyz"));
            list2.Add(new Room("https://www.seekpng.com/png/small/67-679395_youtube-play-png-youtube-2018-logo-png.png", "xyz"));
            list2.Add(new Room("https://tuoitreit.vn/data/avatars/o/88/88344.jpg?1566350734", "xyz"));
            list2.Add(new Room("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ7E3YMQl6YDwv3ccD11ksoB8kHJ3Zd0E2bSZy9whl6nM9Gwy8LjzvUFv-4jHbfy0whOPM&usqp=CAU", "xyz"));
            list2.Add(new Room("https://www.seekpng.com/png/small/67-679395_youtube-play-png-youtube-2018-logo-png.png", "xyz"));
            list2.Add(new Room("https://tuoitreit.vn/data/avatars/o/88/88344.jpg?1566350734", "xyz"));
            list2.Add(new Room("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ7E3YMQl6YDwv3ccD11ksoB8kHJ3Zd0E2bSZy9whl6nM9Gwy8LjzvUFv-4jHbfy0whOPM&usqp=CAU", "xyz"));
            list_Unknown.ItemsSource = list2;
            ShowRoom();
        }

        public async Task<ObservableCollection<Room>> HandleRoom() //Get all room
        {
            try
            {
                var httpService = new HttpService();
                var base_URL = @"http://192.168.30.103:3000/room";
                var result = await httpService.SendAsync(base_URL, HttpMethod.Get);
                var listRoom = JsonConvert.DeserializeObject<ObservableCollection<Room>>(result);

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

        public async void ShowRoom()
        {
            try
            {
                var listRoom = new ObservableCollection<Room>(await HandleRoom());
                foreach (Room room in listRoom)
                {
                    luvm.RoomCollection.Add(room);
                }
                
                this.BindingContext = luvm;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
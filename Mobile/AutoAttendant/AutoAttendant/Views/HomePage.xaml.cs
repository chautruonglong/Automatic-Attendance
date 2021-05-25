﻿using AutoAttendant.Models;
using AutoAttendant.Services;
using AutoAttendant.ViewModel;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
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
    [Obsolete]
    public partial class HomePage : MasterDetailPage
    {
        public static ListRoomViewModel _lrvm = new ListRoomViewModel();
        public static ListScheduleViewModel _lsvm = new ListScheduleViewModel();
        public static ListSubjectViewModel _lsjvm = new ListSubjectViewModel();
        public static int checkCreateListSchedule = 0; //avoid repeat schedule from ShowSchedule()
        public static int checkCreateRoom = 0; //avoid repeat schedule from ShowSchedule()
        public static int checkUpdateSchedule = 0; // check load list schedule again after Update schedule

        //public static Lecture _lecture = new Lecture();
        public static string base_URL = "http://192.168.30.102:3000";
        public HomePage()
        {
            InitializeComponent();
            Detail = new NavigationPage(new SubjectPage());
            GetLectureInfoById(Data.Data.Instance.User.idLecture.ToString());
            HandleRoom();
        }


        public async void HandleRoom()
        {
            try
            {
                var httpService = new HttpService();
                var base_URL = HomePage.base_URL + "/room/";
                var result = await httpService.SendAsync(base_URL, HttpMethod.Get);
                //WebClient wc = new WebClient();
                //var result = wc.DownloadString(full_url);
                var listRoom = JsonConvert.DeserializeObject<ObservableCollection<Room>>(result);
                _lrvm.RoomCollection = listRoom;
            }
            catch (Exception)
            {
                await DisplayAlert("Notice", "Fail", "OK");
            }
        }

        public async void GetLectureInfoById(string id) //lay theo id ben login truyền qua, set up cho profile 
        {
            var httpService = new HttpService();
            var base_URL = HomePage.base_URL + "/lecture/" + id;
            var x = HomePage.base_URL + "/lecture/" + id;
            var result = await httpService.SendAsync(base_URL, HttpMethod.Get);
            var lecture = JsonConvert.DeserializeObject<Lecture>(result);
            Data.Data.Instance.Lecture = lecture;
            Lb_LectureName.Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(lecture.name.ToLower());
            string avaText = "";
            lecture.name.Split(' ').ToList().ForEach(i => avaText += i[0].ToString());
            Avatar.Text = avaText;
            Avatar.TextColor = Color.FromHex("#021135");;
        }

        private void HandleTeacherProfile(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new LectureProfilePage());
            IsPresented = false;
        }

        private void HandleHistory(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new HistoryPage());
            IsPresented = false;
        }

        private void HandleLogOut(object sender, EventArgs e)
        {
            checkCreateListSchedule = 0;
            //_lsvm.ScheduleCollection.Clear();
            HomePage._lsjvm.SubjectCollection.Clear();
            // truoc khi out thi` cap nhat lai vo DB
            Navigation.PopToRootAsync();
        }


        private void HandleHome(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new ClassPage());
            Detail = new NavigationPage(new SubjectPage());
            IsPresented = false;
        }

        async void ChangeAvatar(object sender, EventArgs e)
        {
            try
            {
                await CrossMedia.Current.Initialize();
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await DisplayAlert("Notice", "Picking a photo is not supported", "OK");
                    return;
                }
                var mediaOptions = new PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Medium
                };
                var selectedFile = CrossMedia.Current.PickPhotoAsync(mediaOptions);
                if (selectedFile == null)
                {
                    await DisplayAlert("Error", "Could not get the image from Gallery!", "OK");
                    Avatar.Source = "DefaultAvatar.jpg";
                }
                //Avatar.Source = ImageSource.FromStream(() => selectedFile.GetStream());
                else
                {
                    Avatar.Source = ImageSource.FromStream(() => selectedFile.Result.GetStream());
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Notice", "Picking a photo is not supported", "OK");
            }
            

        }

        private void HandleRoom(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new RoomPage());
            IsPresented = false;
        }

        private void HandleSetting(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new SettingPage());
            IsPresented = false;
        }
    }
}
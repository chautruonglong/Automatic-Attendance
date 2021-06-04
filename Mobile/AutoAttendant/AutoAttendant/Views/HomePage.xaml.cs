using AutoAttendant.Models;
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
using System.Net.Http.Headers;
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
        public static ListSubjectViewModel _lsjvm = new ListSubjectViewModel();     //list subject by day
        public static ListSubjectAllViewModel _lsjavm = new ListSubjectAllViewModel();    //list all subject by lecturer_id
        public static int checkCreateListSubject = 0; //avoid repeat subject from ShowSubject()
        public static int checkCreateRoom = 0; //avoid repeat schedule from ShowSchedule()
        public static int checkUpdateSubject = 0; // check load list subject again after update subject

        public static string base_URL;

        public static ListScheduleViewModel _lsvm = new ListScheduleViewModel(); //k dung`
        //public static Lecture _lecture = new Lecture();
        //public static string base_URL = "http://192.168.30.102:3000";
        //public static string base_URL = "http://192.168.30.104:8000";
        public HomePage()
        {
            InitializeComponent();
            Detail = new NavigationPage(new SubjectTabbedPage());                   //
            GetLectureInfoById(Data.Data.Instance.UserNui.lecturer_id.ToString());  //
            HandleRoom();                                                           //
        }

        public async void HandleRoom()
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
                _lrvm.RoomCollection = listRoom;
            }
            catch (Exception ex)
            {
                await DisplayAlert("ERROR", ex.Message , "OK");
            }
        }

        public async void GetLectureInfoById(string id) //lay theo id ben login truyền qua, set up cho profile 
        {
            try
            {
                //var httpService = new HttpService();
                //var base_URL = HomePage.base_URL + "/lecturer/detail/" + id + "/";
                //var result = await httpService.SendAsync(base_URL, HttpMethod.Get);
                //var lecture = JsonConvert.DeserializeObject<Lecture>(result);

                var httpService = new HttpClient();
                var api_key = Data.Data.Instance.UserNui.authorization;
                httpService.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("authorization", api_key);
                var base_URL = HomePage.base_URL + "/lecturer/detail/" + id + "/";
                var result = await httpService.GetAsync(base_URL);
                var jsonLecturer = await result.Content.ReadAsStringAsync();
                var lecturer = JsonConvert.DeserializeObject<Lecture>(jsonLecturer);

                Data.Data.Instance.Lecture = lecturer;
                Lb_LectureName.Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(lecturer.name.ToLower());
                string avaText = "";
                lecturer.name.Split(' ').ToList().ForEach(i => avaText += i[0].ToString());
                Avatar.Text = avaText.ToUpper();
                Avatar.TextColor = Color.FromHex("#021135");
            }
            catch(Exception ex)
            {
                await DisplayAlert("Notice", ex.Message, "OK");
            }
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

        #region Functions Handle Nav Bar
        private void HandleHome(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new ClassPage());
            Detail = new NavigationPage(new SubjectTabbedPage());
            IsPresented = false;
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
        private void HandleLogOut(object sender, EventArgs e)
        {
            checkCreateListSubject = 0;

            _lsjvm.SubjectCollection.Clear();
            // truoc khi out thi` cap nhat lai vo DB
            Navigation.PopToRootAsync();
        }
        #endregion
    }
}
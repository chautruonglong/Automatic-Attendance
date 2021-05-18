using AutoAttendant.Models;
using AutoAttendant.Services;
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
    public partial class UpdateSchedulePage : ContentPage
    {
        public static Schedule scheduleTemp;

        [Obsolete]
        public UpdateSchedulePage(Schedule schedule)
        {
            InitializeComponent();
            scheduleTemp = schedule;
            HandleDatePicker(schedule);
            GetRoomForUpdateSchedule();
            var room = HomePage._lrvm.RoomCollection.Single(r => r.id == schedule.idRoom);
            PickerRoom.SelectedIndex = Convert.ToInt32(room.id) - 1;
        }

        public void GetTimeSlot(ObservableCollection<Schedule> listSchedule)
        {
            usedTimeSlotLabel.Text = "|";
            foreach (Schedule schedule in listSchedule)
            {

                usedTimeSlotLabel.Text = usedTimeSlotLabel.Text + "   " + schedule.timeSlot + "   " + "|";
            }
        }
        private void HandleDatePicker(Schedule schedule)
        {
            lb_date.Text = schedule.date.ToString().Substring(0, 10);
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

                lb_date.Text = date;
            };
        }

        [Obsolete]
        public async void GetRoomForUpdateSchedule()
        {
            try
            {
                var listRoom = HomePage._lrvm.RoomCollection;
                foreach (Room room in listRoom)
                {
                    PickerRoom.Items.Add(room.name);
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Notice", "Fail", "OK");
            }
        }

        [Obsolete]
        private async void HandlePickerRoom(object sender, EventArgs e)
        {
            var index = PickerRoom.SelectedIndex;
            if (index != -1)
            {
                btnSelectRoom.Text = PickerRoom.Items[index].ToString();
                var room = HomePage._lrvm.RoomCollection.Single(r => r.name == btnSelectRoom.Text);

                var httpService = new HttpService();
                DateTime dateTime = Convert.ToDateTime(lb_date.Text);
                string date = JsonConvert.SerializeObject(dateTime);
                date = date.Replace("\"", "");
                var base_URL = HomePage.base_URL + "schedule?idSubject=" + scheduleTemp.idSubject + "&idRoom=" + room.id + "&date=" + date;
                var result = await httpService.SendAsync(base_URL, HttpMethod.Get);
                var listSchedule = JsonConvert.DeserializeObject<ObservableCollection<Schedule>>(result);

                GetTimeSlot(listSchedule);
            }
        }

        private void OpenPicker(object sender, EventArgs e)
        {
            PickerRoom.IsEnabled = true;
            PickerRoom.Focus();
        }

        [Obsolete]
        private async void CancelUpdatePopup(object sender, EventArgs e)
        {
            //await PopupNavigation.PopAsync();
        }

        [Obsolete]
        private async void SaveUpdatePopUp(object sender, EventArgs e)
        {
            scheduleTemp.date = Convert.ToDateTime(lb_date.Text);
            var room = HomePage._lrvm.RoomCollection.Single(r => r.name == btnSelectRoom.Text);
            scheduleTemp.idRoom = room.id;
            scheduleTemp.timeSlot = Entry_timeSlot1.Text + "," + Entry_timeSlot2.Text;

            var httpService = new HttpClient();
            string jsonSchedule = JsonConvert.SerializeObject(scheduleTemp); // convert object => json
            string colorState = "," + @"""colorState""";
            int removeIndex = jsonSchedule.IndexOf(colorState);
            jsonSchedule = jsonSchedule.Substring(0, removeIndex) + "}";
            StringContent contentLecture = new StringContent(jsonSchedule, Encoding.UTF8, "application/json");
            var baseLecture_URL = HomePage.base_URL + "schedule/" + scheduleTemp.id.ToString();
            HttpResponseMessage responseLecture = await httpService.PutAsync(baseLecture_URL, contentLecture);
            HomePage.checkUpdateSchedule = 1;
            await Navigation.PopAsync();
        }
    }
}
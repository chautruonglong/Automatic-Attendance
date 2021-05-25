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

        public void GetTimeSlot(ObservableCollection<Schedule> listSchedule)  // hien thi tat ca time slot cua Room
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
                if(Convert.ToDateTime(lb_date.Text).Date < DateTime.Today)
                {
                    DisplayAlert("Notice", "You can not select previous days", "Try again");
                }
            };
        }

        [Obsolete]
        public async void GetRoomForUpdateSchedule() // get all rooms in system
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
        private async void HandlePickerRoom(object sender, EventArgs e) // Get all time slots in a room
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
                var base_URL = HomePage.base_URL + "schedule?&idRoom=" + room.id + "&date=" + date;
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
            try
            {
                bool isValidDate = false;
                bool isValidTimeSlot = false;


                var today = DateTime.Today;
                if (Convert.ToDateTime(lb_date.Text).Date < today)
                {
                    await DisplayAlert("Notice", "You can not select previous days", "Try again");
                }
                else
                {
                    // get date
                    scheduleTemp.date = Convert.ToDateTime(lb_date.Text);
                    isValidDate = true;
                }

                var room = HomePage._lrvm.RoomCollection.Single(r => r.name == btnSelectRoom.Text);
                //get id room
                scheduleTemp.idRoom = room.id;

                // get time slot
                try
                {
                    //validate time slot input
                    var start = Convert.ToInt32(Entry_timeSlot1.Text);
                    var end = Convert.ToInt32(Entry_timeSlot2.Text);
                    
                    if (start > 10 || start <= 0 || end > 10  || end <= 0)
                    {
                        await DisplayAlert("Notice", "Time slot must be from 1 to 10!", "Try again");
                    }
                    else if (start > end)
                    {
                        await DisplayAlert("Notice", "Invalid time slot", "Try again");
                    }
                    else if (usedTimeSlotLabel.Text.Contains(start.ToString()) || usedTimeSlotLabel.Text.Contains(end.ToString()))
                    {
                        await DisplayAlert("Notice", "Time slot was registed by other lectures!", "Try again");
                    }
                    else
                    {
                        scheduleTemp.timeSlot = Entry_timeSlot1.Text + "," + Entry_timeSlot2.Text;
                        isValidTimeSlot = true;
                    }
                }
                catch (Exception)
                {
                    await DisplayAlert("Notice", "Time slot must be a number", "Try again");
                }

                if(isValidDate && isValidTimeSlot)
                {
                    var httpService = new HttpClient();
                    string jsonSchedule = JsonConvert.SerializeObject(scheduleTemp); // convert object => json
                    string colorState = "," + @"""colorState""";
                    int removeIndex = jsonSchedule.IndexOf(colorState);
                    jsonSchedule = jsonSchedule.Substring(0, removeIndex) + "}";

                    StringContent contentSchedule = new StringContent(jsonSchedule, Encoding.UTF8, "application/json");
                    var baseLecture_URL = HomePage.base_URL + "schedule/" + scheduleTemp.id.ToString();
                    HttpResponseMessage responseSchedule = await httpService.PutAsync(baseLecture_URL, contentSchedule);
                    if (responseSchedule.IsSuccessStatusCode)
                    {
                        HomePage.checkUpdateSubject = 1;
                        await Navigation.PopAsync();
                    }
                }
                
            }
            catch(Exception ex)
            {
                await DisplayAlert("Notice", ex.Message, "Try again");
            }
            
        }
    }
}
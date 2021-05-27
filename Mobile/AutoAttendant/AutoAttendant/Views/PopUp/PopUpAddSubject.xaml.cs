using AutoAttendant.Models;
using AutoAttendant.Services;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoAttendant.Views.PopUp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopUpAddSubject
    {
        public PopUpAddSubject()
        {
            InitializeComponent();
            HandleDatePicker();
            GetRoomForAddSubject();
        }

        public EventHandler<string> Action;

        private void HandleDatePicker()
        {
            lb_date.Text = DateTime.Now.DayOfWeek.ToString();
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
                lb_date.Text = day;
            };
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
                string day= JsonConvert.SerializeObject(dateTime);
                day = day.Replace("\"", "");
                var base_URL = HomePage.base_URL + "subject?room_id=" + room.id + "&day=" + day;
                var result = await httpService.SendAsync(base_URL, HttpMethod.Get);
                var listSubjectInRoom = JsonConvert.DeserializeObject<ObservableCollection<Subject>>(result);

                GetTimeSlot(listSubjectInRoom);
            }
        }

        public void GetTimeSlot(ObservableCollection<Subject> listSubject)
        {
            usedTimeSlotLabel.Text = "|";
            foreach (Subject subject in listSubject)
            {

                usedTimeSlotLabel.Text = usedTimeSlotLabel.Text + "   " + subject.time_slot + "   " + "|";
            }
        }

        private void OpenPicker(object sender, EventArgs e)
        {
            PickerRoom.IsEnabled = true;
            PickerRoom.Focus();
        }

        [Obsolete]
        public async void GetRoomForAddSubject()
        {
            try
            {
                var listRoom = HomePage._lrvm.RoomCollection;
                foreach (Room room in listRoom)
                {
                    PickerRoom.Items.Add(room.name);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Notice", ex.Message, "OK");
            }
        }

        [Obsolete]
        private async void CancelAddSubjectPopup(object sender, EventArgs e)
        {
            await PopupNavigation.PopAsync();
        }

        private async void SaveNewSubject(object sender, EventArgs e)
        {
            Action?.Invoke(this, "Add");
            await PopupNavigation.Instance.PopAsync();
        }
    }
}
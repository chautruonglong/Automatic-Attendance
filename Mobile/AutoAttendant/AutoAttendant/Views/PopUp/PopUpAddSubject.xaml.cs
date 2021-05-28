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
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoAttendant.Views.PopUp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopUpAddSubject
    {
        [Obsolete]
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
            try
            {
                var index = PickerRoom.SelectedIndex;
                if (index != -1)
                {
                    btnSelectRoom.Text = PickerRoom.Items[index].ToString();
                    var room = HomePage._lrvm.RoomCollection.Single(r => r.name == btnSelectRoom.Text);

                    var httpService = new HttpService();
                    var day = lb_date.Text;
                    var base_URL = HomePage.base_URL + "/subject?room_id=" + room.id + "&day=" + day;
                    var result = await httpService.SendAsync(base_URL, HttpMethod.Get);
                    var listSubjectInRoom = JsonConvert.DeserializeObject<ObservableCollection<Subject>>(result);

                    GetTimeSlot(listSubjectInRoom);
                }
            }
            catch(Exception ex)
            {
                await DisplayAlert("Notice", ex.Message, "OK");
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
                await DisplayAlert("ERROR", ex.Message, "OK");
            }
        }

        [Obsolete]
        private async void SaveNewSubject(object sender, EventArgs e)
        {
            try
            {
                string day = lb_date.Text;
                string room_id = "21";
                string time_slot = "19:30-20:30";
                string name = Entry_subjectName.Text;
                string id_lecturer = Data.Data.Instance.User.idLecture.ToString();

                //Declare Api-Key

                //var api_key = Data.Data.Instance.UserNui.authorization;
                //httpService.DefaultRequestHeaders.Accept.Add("authorization", api_key);
                //httpService.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("authorization", api_key);

                var httpService = new HttpClient();
                var base_SubjectURL = HomePage.base_URL + "/subject/create/";
                var base_StudentListURL = HomePage.base_URL + "/student/list/create/";

                //Post list student from Excel
                var listStudent = new List<StudentNui>();
                string jsonListStudent = JsonConvert.SerializeObject(listStudent);
                StringContent contentListStudent = new StringContent(jsonListStudent, Encoding.UTF8, "application/json");
                HttpResponseMessage responseListStudent = await httpService.PostAsync(base_SubjectURL, contentListStudent);

                //Post new Subject
                var newSubject = new Subject("", id_lecturer, room_id, name, time_slot, day);
                string jsonNewSubject = JsonConvert.SerializeObject(newSubject);
                StringContent contentSubject = new StringContent(jsonNewSubject, Encoding.UTF8, "application/json");
                HttpResponseMessage responseNewSubject = await httpService.PostAsync(base_StudentListURL, contentSubject);

                //Back to Subject Page after Post to server
                if(responseListStudent.IsSuccessStatusCode && responseNewSubject.IsSuccessStatusCode)
                {
                    Action?.Invoke(this, "Add succesfully");
                    await PopupNavigation.Instance.PopAsync();
                }

            }
            catch(Exception ex)
            {
                await DisplayAlert("ERROR", ex.Message, "OK");
            }
            
        }

        private async void ImportExcel(object sender, EventArgs e)
        {
            try
            {
                var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.iOS, new[] {"com.microsoft.xlsx"} },
                    { DevicePlatform.Android, new[] { "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" } },
                });
                var pickerResult = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = customFileType,
                    PickerTitle = "Pick an Excel file"
                });

                if (pickerResult != null)
                {
                    var stream = await pickerResult.OpenReadAsync();
                    var fileName = pickerResult.FileName; // lay ra path file
                    lb_ExcelFile.Text = fileName;
                }
            }
            catch(Exception ex)
            {
                await DisplayAlert("ERROR", ex.Message, "OK");
            }
        }

        public void ValidatePopUpAdd(string name, string roomName, string time_slot, string used_timeSlot, string day)
        {

        }

        [Obsolete]
        private async void CancelAddSubjectPopup(object sender, EventArgs e)
        {
            await PopupNavigation.PopAsync();
        }
    }
}
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
    public partial class HistoryPage : ContentPage
    {
        int checkCreateListSchedule = 0;
        ListScheduleViewModel lsvm = new ListScheduleViewModel();
        [Obsolete]
        public HistoryPage()
        {
            InitializeComponent();
            HandleDatePicker();
            this.BindingContext = new ListScheduleViewModel();
        }

        [Obsolete]
        public async Task<ObservableCollection<Schedule>> HandleScheduleByDate(string date)
        {
            try
            {
                var httpService = new HttpService();
                //var base_URL = HomePage.base_URL + "schedule?idTeacher=" + Data.Data.Instance.User.idLecture.ToString() + "&date=" + date;
                var base_URL = HomePage.base_URL + "/schedule?idTeacher=" + Data.Data.Instance.Lecture.id.ToString() + "&date=" + date;
                var result = await httpService.SendAsync(base_URL, HttpMethod.Get);
                var listSchedule = JsonConvert.DeserializeObject<ObservableCollection<Schedule>>(result);
                return listSchedule;
            }
            catch (Exception)
            {
                await DisplayAlert("Notice", "Fail", "OK");

                return null;
            }
        }

        [Obsolete]
        public async void ShowSchedule(string date)
        {
            try
            {
                if (checkCreateListSchedule == 0)
                {
                    var listSchedule = new ObservableCollection<Schedule>(await HandleScheduleByDate(date)); // list Schedule trả về từ HandelSchedule
                    if(listSchedule.Count == 0)
                    {
                        await DisplayAlert("Notice", "No schedule on this day", "OK");
                    }
                    else foreach (Schedule schedule in listSchedule)  // duyet trong list schedule để thêm vào lsvm
                    {
                        schedule.stateString = "0 / 0"; 
                        lsvm.ScheduleCollection.Add(schedule);

                    }
                }
                //SetColorById();
                //lsvm.ScheduleCollection = HomePage._lsvm.ScheduleCollection; // gán lsvm bên Login cho lsvm của trang này -> avoid add same schedule
                this.BindingContext = lsvm;
            }
            catch (Exception)
            {
                await DisplayAlert("Notice", "Select date to view your history", "OK");
            }
        }

        [Obsolete]
        private void HandleDatePicker()
        {
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
                lb_date.Text = "Schedule on " + day + " " + date;

                //Format DateTime to send GET to server

                string historyDate = JsonConvert.SerializeObject(datePicker.Date).Replace("\"", "");
                lsvm.ScheduleCollection.Clear();
                ShowSchedule(historyDate);
            };
        }


    }
}
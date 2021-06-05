using AutoAttendant.Models;
using AutoAttendant.Services;
using AutoAttendant.ViewModel;
using Newtonsoft.Json;
using Syncfusion.XlsIO;
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
    public partial class HistoryPage : ContentPage
    {
        public static int checkCreateAllSubject = 0;
        [Obsolete]
        public HistoryPage()
        {
            InitializeComponent();
            ShowAllSubject();
        }

        [Obsolete]
        protected override void OnAppearing() // goị trước khi screen page này xuất hiện
        {
            ReLoadSubjectList();
            base.OnAppearing();
        }

        [Obsolete]
        public void ReLoadSubjectList()
        {
            HomePage._lsjavm.SubjectAllCollection.Clear();
            ShowAllSubject();
            this.BindingContext = new ListSubjectAllViewModel();
            this.BindingContext = HomePage._lsjavm;
        }

        [Obsolete]
        public async Task<ObservableCollection<Subject>> HandleAllSubject() //get all subject by lecturer_id
        {
            try
            {
                var httpService = new HttpClient();
                var api_key = Data.Data.Instance.UserNui.authorization;
                httpService.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("authorization", api_key);
                var base_URL = HomePage.base_URL + "/subject/list/" + Data.Data.Instance.UserNui.lecturer_id.ToString() + "/all/";
                var result = await httpService.GetAsync(base_URL);
                var jsonSubject = await result.Content.ReadAsStringAsync();
                var listSubject = JsonConvert.DeserializeObject<ObservableCollection<Subject>>(jsonSubject);

                // order list subject by time slot
                var dayIndex = new List<string> { "MONDAY", "TUESDAY", "WEDNESDAY", "THURSDAY", "FRIDAY", "SATURDAY", "SUNDAY" };
                listSubject = new ObservableCollection<Subject>(listSubject.OrderBy(r => dayIndex.IndexOf(r.day.ToUpper())));
                return listSubject;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Notice", ex.Message, "OK");
                return null;
            }
        }

        [Obsolete]
        public async void ShowAllSubject()
        {
            try
            {
                var listSubject = new ObservableCollection<Subject>(await HandleAllSubject()); // list Subject trả về từ HandelSubject
                if (listSubject.Count > 0)
                {
                    HomePage._lsjavm.SubjectAllCollection.Clear();
                    foreach (Subject subject in listSubject)  // duyet trong list subject để thêm vào lsjavm
                    {
                        HomePage._lsjavm.SubjectAllCollection.Add(subject);
                    }

                    this.BindingContext = new ListSubjectAllViewModel();
                    this.BindingContext = HomePage._lsjavm;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Notice", "Fall in ShowAllSubject /n" + ex.Message, "OK");
            }
        }

        private async void GetExcel(object sender, EventArgs e)
        {
            var httpService = new HttpClient();
            var api_key = Data.Data.Instance.UserNui.authorization;
            httpService.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("authorization", api_key);
            var base_URL = "http://42.114.97.127:8000/resources/class/1814_KTDN.xlsx";
            var result = await httpService.GetAsync(base_URL);
            var streamExcel = await result.Content.ReadAsStreamAsync();

            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            application.DefaultVersion = ExcelVersion.Excel2016;

            //Open the workbook
            IWorkbook workbook = application.Workbooks.Open(streamExcel);
            //Access first worksheet from the workbook.
            IWorksheet worksheet = workbook.Worksheets[0];
            var numberOfStudent = (worksheet.Rows.Count() - 3);
            var inforSubject = worksheet.Range["A5"].Text.Trim();
            var lastindexOfNameSub = inforSubject.IndexOf("(");
            var name_sub = inforSubject.Substring(5, lastindexOfNameSub - 5 - 1);
            var id_sub = inforSubject.Substring(lastindexOfNameSub + 1, inforSubject.Count() - 2 - lastindexOfNameSub).Trim();
            var x = worksheet.Range["B8"].Text.ToString();
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
                //lsvm.ScheduleCollection.Clear();
                //ShowSchedule(historyDate);
            };
        }

        private void SubjectClick(object sender, EventArgs e)
        {

        }
    }
}
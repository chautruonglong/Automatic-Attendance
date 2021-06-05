using AutoAttendant.Models;
using AutoAttendant.ViewModel;
using Newtonsoft.Json;
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
    public partial class ListStudentForViewPage : ContentPage
    {
        public static ListStudentNuiViewModel lsnvm = new ListStudentNuiViewModel();
        public string historyDate = "";

        [Obsolete]
        public ListStudentForViewPage(Subject subject)
        {
            InitializeComponent();
            this.BindingContext = new ListStudentNuiViewModel();
            ShowStudentList(subject.subject_id);
            //historyDate = date;
        }

        private void OnTapped(object sender, EventArgs e)
        {

        }

        [Obsolete]
        public async Task<ObservableCollection<StudentNui>> HandleStudentList(string subject_id) //show list subject today
        {
            try
            {
                var httpService = new HttpClient();
                var api_key = Data.Data.Instance.UserNui.authorization;
                httpService.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("authorization", api_key);
                var base_URL = HomePage.base_URL + "/student/list/" + subject_id + "/";
                var result = await httpService.GetAsync(base_URL);
                var jsonStdList = await result.Content.ReadAsStringAsync();
                var listStudent = JsonConvert.DeserializeObject<ObservableCollection<StudentNui>>(jsonStdList);

                // order list student by name
                listStudent = new ObservableCollection<StudentNui>(listStudent.OrderBy(r => r.name.Split(' ').ToList()[r.name.Split(' ').ToList().Count - 1]));
                return listStudent;
            }
            catch (Exception ex)
            {
                await DisplayAlert("ERROR", ex.Message, "OK");
                return null;
            }
        }

        [Obsolete]
        public async void ShowStudentList(string subject_id)
        {
            try
            {
                lsnvm.StudentCollection.Clear();
                var listAttendance = new ObservableCollection<AttendanceNui>(await GetAttendanceForView(subject_id)); // list Subject trả về từ HandelSubject
                var listStudent = await HandleStudentList(subject_id);
                
                if (listStudent.Count > 0)
                {
                    foreach(AttendanceNui atdNui in listAttendance)
                    {
                        try
                        {
                            var std = listStudent.Single(r => r.student_id == atdNui.id && atdNui.type == "known");
                            std.state = true;
                        }
                        catch (Exception)
                        {

                        }
                    }
                    lsnvm.StudentCollection = listStudent;
                    this.BindingContext = lsnvm;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("ERROR", ex.Message, "OK");
            }

        }

        [Obsolete]
        public async Task<ObservableCollection<AttendanceNui>> GetAttendanceForView(string subject_id)
        {
            try
            {
                var httpService = new HttpClient();
                var api_key = Data.Data.Instance.UserNui.authorization;
                httpService.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("authorization", api_key);
                var date = String.Format("{0:yyyy-MM-dd}", DateTime.Now.Date);
                var base_URL = HomePage.base_URL + "/attendance/history/latest/" + subject_id + "/" + date + "/";
                var result = await httpService.GetAsync(base_URL);
                if (result.IsSuccessStatusCode)
                {
                    var jsonProcess = await result.Content.ReadAsStringAsync();
                    var process_id = JsonConvert.DeserializeObject<string>(jsonProcess);

                    var base2_URL = HomePage.base_URL + "/attendance/list/" + process_id + "/";
                    var result2 = await httpService.GetAsync(base2_URL);
                    var jsonHistoryList = await result2.Content.ReadAsStringAsync();
                    var listAttendance = JsonConvert.DeserializeObject<ObservableCollection<AttendanceNui>>(jsonHistoryList);

                    // order list student by name
                    //listAttendance = new ObservableCollection<StudentNui>(listAttendance.OrderBy(r => r..Split(' ').ToList()[r.name.Split(' ').ToList().Count - 1]));
                    return listAttendance;
                }
                else
                {
                    await DisplayAlert("Notice", "Fail in GetProcessForAttendance ", "OK");
                    return null;
                }
                
            }
            catch(Exception ex)
            {
                await DisplayAlert("ERROR", ex.Message, "OK");
                return null;
            }
        }

       

        private void ToChartPage(object sender, EventArgs e)
        {

        }
    }
}
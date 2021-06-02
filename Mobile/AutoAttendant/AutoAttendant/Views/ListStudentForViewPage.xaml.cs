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
        public ListStudentForViewPage(Subject subject, string date)
        {
            InitializeComponent();
            this.BindingContext = new ListStudentNuiViewModel();
            ShowStudentList(subject.subject_id);
            historyDate = date;
        }

        private void OnTapped(object sender, EventArgs e)
        {

        }

        [Obsolete]
        public async void ShowStudentList(string subject_id)
        {
            try
            {
                lsnvm.StudentCollection.Clear();
                var listStudent = new ObservableCollection<StudentNui>(await HandleHistoryList(subject_id)); // list Subject trả về từ HandelSubject

                if (listStudent.Count > 0)
                {
                    foreach (StudentNui studentNui in listStudent)  // duyet trong listStudent để thêm vào lsvm
                    {
                        lsnvm.StudentCollection.Add(studentNui);
                    }
                    this.BindingContext = lsnvm;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("ERROR", ex.Message, "OK");
            }

        }

        [Obsolete]
        public async Task<string> GetProcessForAttendance(string subject_id)
        {
            try
            {
                var httpService = new HttpClient();
                var api_key = Data.Data.Instance.UserNui.authorization;
                httpService.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("authorization", api_key);
                var base_URL = HomePage.base_URL + "/process/list/" + subject_id + "/" + historyDate + "/";
                var result = await httpService.GetAsync(base_URL);
                if (result.IsSuccessStatusCode)
                {
                    var jsonProcess = await result.Content.ReadAsStringAsync();
                    var process_id = JsonConvert.DeserializeObject<string>(jsonProcess);
                    return process_id;
                }
                else return null;
                
            }
            catch(Exception ex)
            {
                await DisplayAlert("ERROR", ex.Message, "OK");
                return null;
            }
        }

        [Obsolete]
        public async Task<ObservableCollection<StudentNui>> HandleHistoryList(string subject_id) //show list subject today
        {
            try
            {
                var httpService = new HttpClient();
                var api_key = Data.Data.Instance.UserNui.authorization;
                httpService.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("authorization", api_key);
                string process_id = await GetProcessForAttendance(subject_id);
                var base_URL = HomePage.base_URL + "/attendance/list/" + process_id + "/";
                var result = await httpService.GetAsync(base_URL);
                var jsonHistoryList = await result.Content.ReadAsStringAsync();
                var listAttendance = JsonConvert.DeserializeObject<ObservableCollection<StudentNui>>(jsonHistoryList);

                // order list student by name
             

                listAttendance = new ObservableCollection<StudentNui>(listAttendance.OrderBy(r => r.name.Split(' ').ToList()[r.name.Split(' ').ToList().Count - 1]));
                return listAttendance;
            }
            catch (Exception ex)
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
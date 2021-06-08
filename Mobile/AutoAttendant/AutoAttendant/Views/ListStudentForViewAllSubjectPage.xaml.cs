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
    public partial class ListStudentForViewAllSubjectPage : ContentPage
    {
        public ListStudentNuiViewModel lsnvm = new ListStudentNuiViewModel();

        [Obsolete]
        public ListStudentForViewAllSubjectPage(string subject_id)
        {
            InitializeComponent();
            this.BindingContext = new ListStudentNuiViewModel();
            ShowStudentList(subject_id);
        }
        protected override void OnAppearing()
        {
            ReLoadStudenList();
            base.OnAppearing();
        }

        public void ReLoadStudenList()
        {

            if (lsnvm.StudentCollection.Count > 0)
            {
                this.BindingContext = new ListStudentNuiViewModel();
                this.BindingContext = lsnvm;
            }
        }

        [Obsolete]
        public async void ShowStudentList(string subject_id)
        {
            try
            {
                lsnvm.StudentCollection.Clear();
                var listStudent = new ObservableCollection<StudentNui>(await HandleStudentList(subject_id)); // list Subject trả về từ HandelSubject

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
    }
}
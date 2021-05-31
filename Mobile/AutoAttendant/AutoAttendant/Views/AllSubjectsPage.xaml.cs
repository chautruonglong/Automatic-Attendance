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
    public partial class AllSubjectsPage : ContentPage
    {
        [Obsolete]
        public AllSubjectsPage()
        {
            InitializeComponent();
            ShowAllSubject();
        }

        [Obsolete]
        public async Task<ObservableCollection<Subject>> HandleAllSubject() //get all subject by lecturer_id
        {
            try
            {
                var httpService = new HttpClient();
                var api_key = Data.Data.Instance.UserNui.authorization;
                httpService.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("authorization", api_key);
                var base_URL = HomePage.base_URL + "/subject/list/" + Data.Data.Instance.UserNui.lecturer_id.ToString() + "/";
                var result = await httpService.GetAsync(base_URL);
                var jsonSubject = await result.Content.ReadAsStringAsync();
                var listSubject = JsonConvert.DeserializeObject<ObservableCollection<Subject>>(jsonSubject);

                // order list subject by time slot
                listSubject = new ObservableCollection<Subject>(listSubject.OrderBy(r => r.time_slot));
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
                if (HomePage.checkCreateListSubject == 0 || HomePage.checkUpdateSubject == 1)
                {
                    var listSubject = new ObservableCollection<Subject>(await HandleAllSubject()); // list Subject trả về từ HandelSubject

                    foreach (Subject subject in listSubject)  // duyet trong list subject để thêm vào lsjavm
                    {
                        HomePage._lsjvm.SubjectCollection.Add(subject);
                    }
                    HomePage._lsjvm.SubjectCollection[0].colorState = "#246CFE";
                    HomePage.checkCreateListSubject = 1;
                    HomePage.checkUpdateSubject = 0;
                }

                this.BindingContext = new ListSubjectViewModel();
                this.BindingContext = HomePage._lsjavm;
            }
            catch (Exception)
            {
                await DisplayAlert("Notice", "No subject today!", "OK");
            }
        }

        private void SubjectClick(object sender, EventArgs e)
        {

        }
    }
}
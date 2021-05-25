using AutoAttendant.Models;
using AutoAttendant.Services;
using AutoAttendant.ViewModel;
using AutoAttendant.Views.PopUp;
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

namespace AutoAttendant.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubjectPage : ContentPage
    {
        [Obsolete]
        //ListSubjectViewModel lsjvm = HomePage._lsjvm;
        public static ListClassViewModel _lcvm = new ListClassViewModel();
        public static Classes classes = new Classes();
        public static int checkClearStd_ListPage = 0; // = 0 thì khi vào lại lớp vẫn còn list student, = 1 thì clear
        public static int first_id_in_list; // id của shedule có thể join dc
        public static string enableSubJectId ; //id của subject có thể join dc

        [Obsolete]
        public SubjectPage()
        {
            InitializeComponent();
            ShowSubject();
        }

        [Obsolete]
        protected override void OnAppearing() // goị trước khi screen page này xuất hiện
        {
            ReLoadScheduleList();
            ReloadPieChart();
            base.OnAppearing();
        }

        [Obsolete]
        public void ReLoadScheduleList()
        {
            if (HomePage._lsjvm.SubjectCollection.Count > 0)
            {
                this.BindingContext = new ListSubjectViewModel();
                SetColorById();
                this.BindingContext = HomePage._lsjvm;
            }
        }

        public void ReloadPieChart()
        {
            ChartPage.absentees = 0;
            ChartPage.attendances = 0;
        }

        public async Task<List<Process>> HandleProcess(string idSub) //Get process (theo list)
        {
            try
            {
                var httpService = new HttpService();
                string date = JsonConvert.SerializeObject(DateTime.Today);
                date = date.Substring(1, 19);
                var base_URL = HomePage.base_URL + "/process?id_subject="+ idSub +"&date=" + date;
                var result = await httpService.SendAsync(base_URL,HttpMethod.Get);
                //if (result != null)
                //{
                    //var  process = new Process(-1, "-1", -1, DateTime.Now, "-1");
                    var process = JsonConvert.DeserializeObject<List<Process>>(result);
                    return process;
                //}
                //else return null;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Notice", "Fail in HandleProcess \n" + ex.Message, "OK");
                return null;
            }
        }
        public async Task<string> GetEnableSubJectId(ObservableCollection<Subject> listSubject) 
        {   
            int step = 0;
            while (step < listSubject.Count)
            {
                List<Process>  list_process = await HandleProcess(listSubject[step].id_subject); // list_process max count = 1 for each subject
                if (list_process.Count == 0)
                {
                    return listSubject[step].id_subject;
                }
                else if (list_process.Count > 0 && list_process[0].status == 0)
                {
                    return listSubject[step].id_subject;
                }
                else if (list_process.Count > 0 && list_process[0].status == 1)
                {
                    step++;
                }
            }
            return null;
        }

        [Obsolete]
        public async Task<ObservableCollection<Subject>> HandleSubject() //show list subject today
        {
            try
            {
                var httpService = new HttpService();
                
                var base_URL = HomePage.base_URL + "/subject?id_lecturer="+ Data.Data.Instance.User.idLecture.ToString() + "&day=" + DateTime.Now.DayOfWeek.ToString();
                var result = await httpService.SendAsync(base_URL, HttpMethod.Get);
                var listSubject = JsonConvert.DeserializeObject<ObservableCollection<Subject>>(result);

                // order list subject by time slot
                listSubject = new ObservableCollection<Subject>(listSubject.OrderBy(r => r.timeSlot));
                return listSubject;
            }
            catch (Exception)
            {
                await DisplayAlert("Notice", "Fail", "OK");

                return null;
            }
        }

        [Obsolete]
        public async void ShowSubject()
        {
            try
            {
                if (HomePage.checkCreateListSubject == 0 || HomePage.checkUpdateSubject == 1)
                {
                    var listSubject = new ObservableCollection<Subject>(await HandleSubject()); // list Subject trả về từ HandelSubject
                    enableSubJectId = await GetEnableSubJectId(listSubject);
                    if(enableSubJectId == null)
                    {
                        await DisplayAlert("Notice", "No subject today!", "OK");
                    }

                    foreach (Subject subject in listSubject)  // duyet trong list subject để thêm vào lsjvm
                    {
                        
                         if (enableSubJectId !=null)
                         {
                            if (subject.id_subject == enableSubJectId)
                            {
                                subject.colorState = "#246CFE";  //luon gan' cho subject dau tien trong list active
                            }
                         }
                        subject.stateString = "0 / 0"; 
                        HomePage._lsjvm.SubjectCollection.Add(subject);
                    }
                    HomePage.checkCreateListSubject = 1;
                    HomePage.checkUpdateSubject = 0;
                }

                SetColorById();
                //this.BindingContext = new ListSubjectViewModel();
                this.BindingContext = HomePage._lsjvm;
            }
            catch (Exception)
            {
                await DisplayAlert("Notice", "No subject today!", "OK");
            }
        }

        [Obsolete]
        public void SetColorById()
        {
            if (enableSubJectId != "-1")
            {
                var item = HomePage._lsjvm.SubjectCollection.Single(r => r.id_subject == enableSubJectId); // tìm subject có Id = enableSubjectID
                item.colorState = "#246CFE"; // set color
                int index = HomePage._lsjvm.SubjectCollection.IndexOf(item); // lấy ra index của subject vừa tìm dc
                if (index > 0)
                {
                    HomePage._lsjvm.SubjectCollection[index - 1].colorState = "#0E368B";
                }
            }
            else // set gia tri cho last subject
            {
                var item = HomePage._lsjvm.SubjectCollection[HomePage._lsjvm.SubjectCollection.Count - 1];
                item.colorState = "#0E368B";
            }
        }

        #region Các hàm xử lí cho SubjectClick
        public async void HandleSelectPopUp(string paraString, Subject subject)
        {
            switch (paraString)
            {
                case "Join":
                    classes.Name = subject.id_subject; // gán cho biến static classes = class tương ứng của subject 

                    if (checkClearStd_ListPage == 1)
                    {
                        classes.StudentList1.Clear(); // clear list student khi join 1 subject mới
                        checkClearStd_ListPage = 0;
                    }
                    await Navigation.PushAsync(new ListStudentPage());
                    break;
                default: //cancel
                    return;
            }
        }


        [Obsolete]
        private async void SubjectClick(object sender, EventArgs e)
        {
            try
            {
                string message = string.Empty;
                Frame f = sender as Frame;
                var fContent = f.Content; // Lấy Content của Frame
                var myStacklayout = fContent.GetType(); // lấy kiểu của Content
                if (myStacklayout == typeof(StackLayout)) // check kiểu có phải Stack Layout ko
                {
                    StackLayout fStacklayout = (StackLayout)fContent;
                    var listChildren = fStacklayout.Children; // Lấy tập Children của StackLayout
                    var firstLabel = listChildren[0];
                    var secondLabel = listChildren[1];
                    var thirdLabel = listChildren[2];
                    var forthLabel = listChildren[3];
                    if (firstLabel.GetType() == typeof(Label) && secondLabel.GetType() == typeof(Label) && thirdLabel.GetType() == typeof(Label))
                    {
                        Label ClassName = (Label)firstLabel;
                        Label Subject = (Label)secondLabel;
                        Label TimeSlot = (Label)thirdLabel;
                        Label LabelId = (Label)forthLabel;
                        if (ClassName.Text == enableSubJectId) // chỉ dc mở subject có id đang = first id in list
                        {
                            var itemSelected = HomePage._lsjvm.SubjectCollection.Single(r => r.id_subject == ClassName.Text && r.name == Subject.Text);
                            var index = HomePage._lsjvm.SubjectCollection.IndexOf(itemSelected); // lấy index của subject đó trong lsvm
                            Subject subject = HomePage._lsjvm.SubjectCollection[index]; // tìm dc schdule theo index

                            var page = new PopUpAddClass(subject);
                            page.Action += async (sender1, stringparameter) =>
                            {
                                HandleSelectPopUp(stringparameter, subject);

                            };
                            await PopupNavigation.Instance.PushAsync(page);
                        }
                        else
                        {
                            await DisplayAlert("Notice", "Your current class must be finished!", "OK");
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }
        #endregion
    }
}
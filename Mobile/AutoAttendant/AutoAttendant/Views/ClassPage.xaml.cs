using AutoAttendant.Models;
using AutoAttendant.Services;
using AutoAttendant.ViewModel;
using AutoAttendant.Views.PopUp;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoAttendant.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClassPage : ContentPage
    {
        [Obsolete]
        ListScheduleViewModel lsvm = HomePage._lsvm;
        public static ListClassViewModel _lcvm = new ListClassViewModel();
        public static Classes classes = new Classes();
        public static int checkClearStd_ListPage = 0; // = 0 thì khi vào lại lớp vẫn còn list student, = 1 thì clear
        public static int first_id_in_list; // id của schedule có thể join dc

        [Obsolete]
        public ClassPage()
        {
            InitializeComponent();
            this.BindingContext = new ListScheduleViewModel();  //page này sẽ binding theo ListScheduleVM
            ShowSchedule();
            
        }

        [Obsolete]
        protected override void OnAppearing() // goị trước khi screen page này xuất hiện
        {
            if(HomePage.checkUpdateSchedule == 1) //co Update xay ra thi khi back ve` phai goi lai API de load lai list schedule
            {
                lsvm.ScheduleCollection.Clear();
                ShowSchedule();
            }
            ReLoadScheduleList();
            ReloadPieChart();
            base.OnAppearing();
        }

        public void ReloadPieChart()
        {
            ChartPage.absentees = 0;
            ChartPage.attendances = 0;
        }

        [Obsolete]
        public void ReLoadScheduleList()
        {
            //SetColorById();
            lsvm.ScheduleCollection = HomePage._lsvm.ScheduleCollection;
            if (lsvm.ScheduleCollection.Count > 0)
            {
                
                this.BindingContext = new ListScheduleViewModel();
                this.BindingContext = lsvm;
                SetColorById();

            }
        }

        [Obsolete]
        public async void HandleSelectPopUp(string paraString, Schedule schedule)
        {
            switch (paraString)
            {
                case "Join":
                    classes.Name = schedule.idSubject; // gán cho biến static classes = class tương ứng của schedule 

                    if (checkClearStd_ListPage == 1)
                    {   

                        classes.StudentList1.Clear(); // clear list student khi join 1 schedule mới
                        checkClearStd_ListPage = 0;
                        
                    }
                    await Navigation.PushAsync(new ListStudentPage());
                    break;
                case "Update":
                    await Navigation.PushAsync(new UpdateSchedulePage(schedule));
                    break;
                case "Delete":
                    int index = HomePage._lsvm.ScheduleCollection.IndexOf(schedule);
                    if (index < HomePage._lsvm.ScheduleCollection.Count - 1) // nếu index của schedule vẫn còn nằm trong _lsvm
                    {
                        ClassPage.first_id_in_list = Convert.ToInt32(HomePage._lsvm.ScheduleCollection[index + 1].id); // gán first id in list = id của schedule tiếp theo
                        ClassPage.checkClearStd_ListPage = 1; // = 1 để khi back về chọn schedule mới sẽ clear list student cũ
                    }
                    else
                    {
                        ClassPage.first_id_in_list = -1; // nếu index vượt thì gán = -1 để ko làm gì khi back về
                    }

                    //Xoa trong DB
                    var httpService = new HttpClient();
                    var base_URL = HomePage.base_URL + "schedule/"+schedule.id.ToString();
                    HttpResponseMessage response = await httpService.DeleteAsync(base_URL);

                    //xoa DB xong -> xoa trong list
                    if (response.IsSuccessStatusCode)
                    {
                        HomePage._lsvm.ScheduleCollection.Remove(schedule);
                        ReLoadScheduleList();
                    }
                    break;
                default: //cancel
                    return;
            }
        }
        [Obsolete]
        private async void ClassClick(object sender, EventArgs e)
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
                        if(Convert.ToInt32(LabelId.Text) == first_id_in_list) // chỉ dc mở schedule có id đang = first id in list
                        {
                            var itemSelected = lsvm.ScheduleCollection.Single(r => r.idSubject == ClassName.Text && r.nameSubject == Subject.Text);
                            var index = lsvm.ScheduleCollection.IndexOf(itemSelected); // lấy index của schedule đó trong lsvm
                            Schedule schedule = lsvm.ScheduleCollection[index]; // tìm dc schdule theo index

                            message = string.Format("Id Room: {0} \nClass: {1} \nSubject: {2} \nTime Slot: {3} \nState: {4}", schedule.idRoom, schedule.idSubject, schedule.nameSubject, schedule.timeSlot, schedule.state);
                            //bool answer = await DisplayAlert("Schedule Info", message, "Join", "Cancel");
                            //if (answer)
                            //{
                            //    classes.Name = schedule.idSubject; // gán cho biến static classes = class tương ứng của schedule 
                            //    //classes.StudentList1.Clear();
                            //    if(checkClearStd_ListPage == 1)
                            //    {
                            //        classes.StudentList1.Clear(); // clear list student khi join 1 schedule mới
                            //        checkClearStd_ListPage = 0;
                            //    }
                            //    await Navigation.PushAsync(new ClassTabbedPage(classes));
                            //}
                            //else
                            //{
                            //    return;
                            //}
                            string roomName = String.Empty;

                            // Handle data from PopUpAddClass
                            var page = new PopUpAddClass(schedule);
                            page.Action += async (sender1, stringparameter) =>
                            {
                                HandleSelectPopUp(stringparameter, schedule);

                            };

                            //page.Disappearing += (c, d) =>
                            //{
                            //    if (roomName != null)
                            //    {

                            //    }
                            //};

                            // call PopUpAddClass
                            await PopupNavigation.Instance.PushAsync(page);

                        }
                        else
                        {
                            await DisplayAlert("Notice","Your current class must be finished!","OK");
                        }
                    }
                }
            }
            catch (Exception)
            {
                
            }
        }


        [Obsolete]
        private async void AddRoom(object sender, EventArgs e)
        {
            await PopupNavigation.PopAsync();

        }

        [Obsolete]
        public async Task<ObservableCollection<Schedule>> HandleSchedule() //show schedule today
        {
            try
            {
                var httpService = new HttpService();
                string date = JsonConvert.SerializeObject(DateTime.Today);
                date = date.Substring(1, 19);
                var base_URL = HomePage.base_URL + "/schedule?idTeacher="+ Data.Data.Instance.Lecture.id_lecturer.ToString() +"&date=" + date + "&state=0";
                var result = await httpService.SendAsync(base_URL, HttpMethod.Get);
                var listSchedule = JsonConvert.DeserializeObject<ObservableCollection<Schedule>>(result);

                // order schedule by time slot
                listSchedule = new ObservableCollection<Schedule>(listSchedule.OrderBy(r => r.timeSlot));
                return listSchedule;
            }
            catch (Exception)
            {
                await DisplayAlert("Notice", "Fail", "OK");
                
                return null;
            }
        }

        [Obsolete]
        public void SetColorById()
        {   
            if (first_id_in_list!=-1) {
                var item = HomePage._lsvm.ScheduleCollection.Single(r => r.id == first_id_in_list); // tìm nút schedule có Id = first id in list
                item.colorState = "#246CFE"; // set color
                int index = HomePage._lsvm.ScheduleCollection.IndexOf(item); // lấy ra index của schedule vừa tìm dc
                if (index > 0) { 
                    HomePage._lsvm.ScheduleCollection[index - 1].colorState = "#0E368B"; 
                }
                
            }
            else // set gia tri cho last schedule
            {
                var item = HomePage._lsvm.ScheduleCollection[HomePage._lsvm.ScheduleCollection.Count - 1];
                item.colorState = "#0E368B";
            }
        }

        [Obsolete]
        public async void ShowSchedule()
        {
            try
            {   if (HomePage.checkCreateListSchedule == 0 || HomePage.checkUpdateSchedule == 1) { 
                    var listSchedule = new ObservableCollection<Schedule>(await HandleSchedule()); // list Schedule trả về từ HandelSchedule

                    first_id_in_list = Convert.ToInt32(listSchedule[0].id);

                    foreach (Schedule schedule in listSchedule)  // duyet trong list schedule để thêm vào lsvm
                    {
                        if(schedule.id == first_id_in_list) 
                        {
                            schedule.colorState = "#246CFE";  //luon gan' cho schedule dau tien trong list active
                        }
                        schedule.stateString = "0 / 0"; // change here
                        HomePage._lsvm.ScheduleCollection.Add(schedule);


                    }
                    HomePage.checkCreateListSchedule = 1;
                    HomePage.checkUpdateSchedule = 0;
                }

                SetColorById();
                

                lsvm.ScheduleCollection = HomePage._lsvm.ScheduleCollection; // gán lsvm bên Home cho lsvm của trang này -> avoid add same schedule
                this.BindingContext = lsvm; 
            }
            catch (Exception)
            {
                await DisplayAlert("Notice", "No schedule today!", "OK");
            }
            
        }

        [Obsolete]
        private  void ShowListClass(object sender, EventArgs e) // 
        {
            //string className = String.Empty;
            //string subject = String.Empty;
            //string timeSlot = String.Empty;

            //var listClass = new ObservableCollection<Schedule>(await HandleSchedule()); // list Schedule 

            //foreach(Schedule classs in listClass)
            //{
            //    className = classs.Classes;
            //    subject = classs.Subject;
            //    timeSlot = classs.TimeSlot;
            //    var itemClass = className + "- " + subject + "- [ " + timeSlot + " ]";
            //    PickerClass.Items.Add(itemClass);

            //}
            //PickerClass.IsEnabled = true;
            //PickerClass.Focus();

            //var page = new PopUpAddClass();
            //page.Action += async (sender1, stringparameter) =>
            //{

            //    if(stringparameter != null)
            //    {
            //        className = stringparameter; // get data tu` PopUp
            //        className = className.Replace(" ", "");
            //        className = className.ToUpper();
            //        message = className + " was added successfully";

            //        Classes classs = new Classes(className);
            //        lcvm.ClassCollection.Add(classs);
            //        this.BindingContext = lcvm;


            //        await DisplayAlert("Notice", message, "OK");

            //    }
            //    else
            //    {
            //        await DisplayAlert("Notice", "Invalid Syntax" , "OK");
            //        return;
            //    }
            //};

            //page.Disappearing += (c, d) =>
            //{
            //    if (className != null)
            //    {

            //    }
            //};
            //await PopupNavigation.Instance.PushAsync(page);
            ////PopupNavigation.PushAsync(new PopUpView());
        }

        //private void DeleteRoom(object sender, EventArgs e)
        //{
        //    Image img = sender as Image;
        //    var stackLayout = img.Parent;
        //    var checkStack = stackLayout.GetType();
        //    if (checkStack == typeof(StackLayout))
        //    {
        //        StackLayout container = (StackLayout)stackLayout;
        //        var listChild = container.Children;

        //        var lb_room = listChild[0].GetType();
        //        if (lb_room == typeof(Label))
        //        {
        //            Label lb = (Label)listChild[0];

        //            var itemToRemove = lsvm.ScheduleCollection.Single(r => r.Classes == lb.Text);
        //            lsvm.ScheduleCollection.Remove(itemToRemove);
        //        }
        //        else
        //        {
        //            DisplayAlert("Fail", "Fail", "Try Again");
        //        }
        //    }
        //}

        //private async void HandlePickerClass(object sender, EventArgs e)
        //{
        //    var index = PickerClass.SelectedIndex;
        //    var listClass = new ObservableCollection<Schedule>(await HandleSchedule()); // list Schedule 

        //    if (index != -1)
        //    {
        //        lsvm.ScheduleCollection.Add(listClass[index]);
        //        this.BindingContext = lsvm;
        //    }
        //}


    }
}
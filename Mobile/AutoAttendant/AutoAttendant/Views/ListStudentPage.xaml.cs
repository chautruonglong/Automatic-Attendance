using Acr.UserDialogs;
using AutoAttendant.Models;
using AutoAttendant.ViewModel;
using Newtonsoft.Json;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace AutoAttendant.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListStudentPage : ContentPage
    {
        public static ListStudentViewModel lsvm = new ListStudentViewModel();     /// Lưu ý coi chừng saiii\

        public ListStudentPage()
        {
            InitializeComponent();
            this.BindingContext = new ListStudentViewModel(); // listview se binding theo object List Student View Model
            //DisplayAlert("NOtice", HomePage._lsvm.ScheduleCollection.Count.ToString(), "OK");
            //ReLoadStudenList();
        }

        protected override void OnAppearing()
        {
            ReLoadStudenList();
            base.OnAppearing();
        }
        public void ReLoadStudenList() // 
        {
            if (lsvm.StudentCollection.Count > 0)
            {
                this.BindingContext = new ListStudentViewModel();
                this.BindingContext = lsvm;
                //DisplayAlert("NOtice", lsvm.StudentCollection[0].State.ToString(), "OK");
            }
        }

        private void OnTapped(object sender, EventArgs e) // xu li khi nhan vao student
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

                //var isLabel = firstLabel.GetType(); // check kiểu của child đầu tiên
                if (firstLabel.GetType() == typeof(Label) && secondLabel.GetType() == typeof(Label) && thirdLabel.GetType() == typeof(Label))
                {
                    Label Name = (Label)firstLabel;
                    Label Class = (Label)secondLabel;
                    Label Time = (Label)thirdLabel;

                    var itemSelected = lsvm.StudentCollection.Single(r => r.Name == Name.Text);
                    var index = lsvm.StudentCollection.IndexOf(itemSelected);
                    Student std = lsvm.StudentCollection[index];
                    message = string.Format("Name: {0}\nClass: {1}\nTime: {2}", std.Id, std.Name, std.Phone);
                    DisplayAlert("Notice", message, "OK");
                    Navigation.PushAsync(new StudentDetailPage(std, lsvm));
                }
            }
        }

        //private void OpenPicker(object sender, EventArgs e)
        //{
        //    PickerSort.IsEnabled = true;
        //    PickerSort.Focus();
        //}

        //private void HandlePickerSort(object sender, EventArgs e) // xu li sort option
        //{
        //    var index = PickerSort.SelectedIndex;
        //    if(index != -1)
        //    {
        //        btnSortOption.Text = PickerSort.Items[index].ToString();
        //    }
        //}
        async void ImportExcel(object sender, EventArgs e) // xu li import excel them student
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
                    //FileTypes = FilePickerFileType.Images,
                    FileTypes = customFileType,
                    PickerTitle = "Pick an Excel file"
                });

                if (pickerResult != null)
                {
                    //var stream = await pickerResult.OpenReadAsync();
                    //resultImg.Source = ImageSource.FromStream(() => stream);
                    //var resourcePath = pickerResult.FullPath.ToString(); // lay ra path file
                    //await DisplayAlert("Message", "Path" + resourcePath, "OK");

                    ExcelEngine excelEngine = new ExcelEngine();
                    IApplication application = excelEngine.Excel;
                    application.DefaultVersion = ExcelVersion.Excel2016;
                    var fileStream = await pickerResult.OpenReadAsync();

                    //Open the workbook
                    IWorkbook workbook = application.Workbooks.Open(fileStream);

                    //Access first worksheet from the workbook.
                    IWorksheet worksheet = workbook.Worksheets[0];
                    var numberOfStudent = (worksheet.Rows.Count() - 3);
                    for (int i = 8; i <= numberOfStudent; i++)
                    {
                        string id = "B" + i.ToString();
                        string name = "C" + i.ToString();
                        string phone = "D" + i.ToString();
                        var mess1 = worksheet.Range[id].Text.ToString();
                        var mess2 = worksheet.Range[name].Text.ToString();
                        var mess3 = worksheet.Range[phone].Text.ToString();

                        try
                        {
                            Student student = new Student(mess1, mess2, "18TCLC-DT2", "IT", mess3, "url", false);
                            ClassPage.classes.StudentList1.Add(student);
                        }
                        catch (Exception ex)
                        {
                            await DisplayAlert("Notice", ex.Message, "OK");
                        }
                    }
                    //ClassPage.classes.StudentList1.Add
                    lsvm.StudentCollection = ClassPage.classes.StudentList1;
                    this.BindingContext = lsvm;
                    //MemoryStream stream1 = new MemoryStream();
                    //workbook.SaveAs(stream1);

                    //workbook.Close();
                    //excelEngine.Dispose();

                }
            }
            catch (Exception)
            {
                await DisplayAlert("Error", "Can't import this file", "OK");
            }
        }

        //private void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        //{
        //    string value = string.Empty;
        //    CheckBox cbStatus = sender as CheckBox;
        //    if (cbStatus.IsChecked == true)
        //    {
        //        btn_Excel.BackgroundColor = Color.FromHex("#246CFE");
        //    }
        //    else btn_Excel.BackgroundColor = Color.FromHex("#021135");
        //}

        private void AddSingleStudent(object sender, EventArgs e) // xu li khi them tung student
        {

        }

        private async void TakeAttendance(object sender, EventArgs e)
        {
            if (lsvm.StudentCollection.Count <= 0)
            {

                await DisplayAlert("Notice", "No students in class. Import student list!", "OK");
            }
            else
            {
                using (IProgressDialog progress = UserDialogs.Instance.Progress("Taking attendance...", null, null, true, MaskType.Gradient))
                {
                    for (int i = 0; i < 100; i++)
                    {
                        progress.PercentComplete = i;
                        await Task.Delay(100);
                    }
                }

                UserDialogs.Instance.Toast("Done");
            }
        }
        public void HandleAttendance()
        {

        }

        [Obsolete]
        public async void HandlePutStateSchedule(Schedule schedule) //update  schedule to server
        {
            var httpService = new HttpClient();
            string jsonSchedule = JsonConvert.SerializeObject(schedule); // convert object => json
            string colorState = "," + @"""colorState""";
            int removeIndex = jsonSchedule.IndexOf(colorState);
            jsonSchedule = jsonSchedule.Substring(0, removeIndex) + "}";
            StringContent contentLecture = new StringContent(jsonSchedule, Encoding.UTF8, "application/json");
            var baseLecture_URL = HomePage.base_URL + "schedule/" + schedule.id.ToString();
            HttpResponseMessage responseLecture =  await httpService.PutAsync(baseLecture_URL, contentLecture);
        }

        public async void HandlePutStateRoom(Schedule schedule) //update  schedule to server
        {
            var roomNow = HomePage._lrvm.RoomCollection.Single(r => r.Id == schedule.idRoom);
            roomNow.State = "Available";
            var httpService = new HttpClient();
            string jsonRoom= JsonConvert.SerializeObject(roomNow); // convert object => json
            StringContent contentLecture = new StringContent(jsonRoom, Encoding.UTF8, "application/json");
            var baseLecture_URL = HomePage.base_URL + "room/" + roomNow.Id.ToString();
            HttpResponseMessage responseLecture = await httpService.PutAsync(baseLecture_URL, contentLecture);
        }

        [Obsolete]
        private async void ClickSaveAndImport(object sender, EventArgs e)
        {
            string className;
            string timeSlot;
            string subject;
            int attendanceCount = 0;

            var schedule = HomePage._lsvm.ScheduleCollection.Single(r => Convert.ToInt32(r.id) == ClassPage.first_id_in_list);
            int index = HomePage._lsvm.ScheduleCollection.IndexOf(schedule);

            
            className = ClassPage.classes.Name;
            timeSlot = schedule.timeSlot;
            subject = schedule.nameSubject;
            foreach(Student std in lsvm.StudentCollection)
            {
                if(std.State == true)
                {
                    attendanceCount++;
                }
            }
            
           
            var message = String.Format("Class: {0}\n Subject: {1}\nTime Slot: {2}\nAttendance: {3}", className, subject, timeSlot, attendanceCount);
            bool answer = await DisplayAlert("Class Info", message, "Save", "Cancel");
            if (answer)
            {
                if (index < HomePage._lsvm.ScheduleCollection.Count - 1) // nếu index của schedule vẫn còn nằm trong _lsvm
                {
                    ClassPage.first_id_in_list = Convert.ToInt32(HomePage._lsvm.ScheduleCollection[index + 1].id); // gán first id in list = id của schedule tiếp theo
                    ClassPage.checkClearStd_ListPage = 1; // =1 để khi back về chọn schedule mới sẽ clear list student cũ
                    schedule.state = 1; // state = 1 là schdule này done
                    schedule.stateString = attendanceCount.ToString() + " / " + lsvm.StudentCollection.Count.ToString();


                }
                else { 
                    ClassPage.first_id_in_list = -1; // nếu index vượt thì gán = -1 để ko làm gì khi back về
                    schedule.state = 1;
                    schedule.stateString = attendanceCount.ToString() + " / " + lsvm.StudentCollection.Count.ToString();
                }

                // Put to Server
                HandlePutStateSchedule(schedule); //cap nhat state cua Schedule
                HandlePutStateRoom(schedule);    //cap nhat state cua Schedule
                await Navigation.PopAsync();
            }
            else
            {
                
            }
        }
    }
}
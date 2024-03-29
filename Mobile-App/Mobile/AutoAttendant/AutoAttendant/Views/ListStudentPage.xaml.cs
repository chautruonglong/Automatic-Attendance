﻿using Acr.UserDialogs;
using AutoAttendant.Models;
using AutoAttendant.Services;
using AutoAttendant.ViewModel;
using AutoAttendant.Views.PopUp;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading;
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
        public static ListStudentNuiViewModel lsnvm = new ListStudentNuiViewModel();
        public static ObservableCollection<Attendance> listAttendance = new ObservableCollection<Attendance>();
        public List<AttendanceNui> listUnknownImage = new List<AttendanceNui>();
        public List<StudentNui> listNotYetAtd = new List<StudentNui>();
        int CheckSquence = 0;

        [Obsolete]
        public ListStudentPage(Subject subject)
        {

            InitializeComponent();
            //this.BindingContext = new ListStudentViewModel(); // listview se binding theo object List Student View Model

            this.BindingContext = new ListStudentNuiViewModel();
            ShowStudentList(subject.subject_id); //goi student list xuong'
        }

        protected override void OnAppearing()
        {
            ReLoadStudenList();
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        
        public void ReLoadStudenList() 
        {

            if (lsnvm.StudentCollection.Count > 0)
            {
                this.BindingContext = new ListStudentNuiViewModel();
                this.BindingContext = lsnvm;
            }
        }


        #region Functions Add for List Students

        [Obsolete]
        public async void ShowStudentList(string subject_id)
        {
            try
            {
                lsnvm.StudentCollection.Clear();
                var listStudent = new ObservableCollection<StudentNui>(await HandleStudentList(subject_id)); // list Subject trả về từ HandelSubject

                if(listStudent.Count > 0)
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
                //var httpService = new HttpService();
                //var base_URL = HomePage.base_URL + "/student/list/" + subject_id + "/";
                //var result = await httpService.SendAsync(base_URL, HttpMethod.Get);
                //var listStudent = JsonConvert.DeserializeObject<ObservableCollection<StudentNui>>(result);

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
                    if (lsvm.StudentCollection.Count == 0) // chua co ds thi` import
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
                                SubjectPage.classes.StudentList1.Add(student);
                            }
                            catch (Exception ex)
                            {
                                await DisplayAlert("Notice", ex.Message, "OK");
                            }
                        }
                        //ClassPage.classes.StudentList1.Add
                        lsvm.StudentCollection = SubjectPage.classes.StudentList1;
                        this.BindingContext = lsvm;
                    }
                    else //co roi` thi block
                    {
                        await DisplayAlert("Notice", "Just import only 1 file", "OK");
                    }
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Error", "Can't import this file", "OK");
            }
        }

        private async void AddSingleStudent(object sender, EventArgs e) // xu li khi them tung student
        {
            var page = new PopUpView();
            page.Action += async (sender1, stringparameter) =>
            {

                if (stringparameter != null)
                {
                    
                }
                else
                {
                    await DisplayAlert("Notice", "Invalid Syntax", "OK");
                    return;
                }
            };
            await PopupNavigation.Instance.PushAsync(page);
        }
#endregion

        private void OnTapped(object sender, EventArgs e) // xu li khi nhan vao student
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

                    //var isLabel = firstLabel.GetType(); // check kiểu của child đầu tiên
                    if (firstLabel.GetType() == typeof(Label) && secondLabel.GetType() == typeof(Label) && thirdLabel.GetType() == typeof(Label))
                    {
                        Label Name = (Label)firstLabel;
                        Label Class = (Label)secondLabel;
                        Label Time = (Label)thirdLabel;

                        var itemSelected = lsnvm.StudentCollection.Single(r => r.name.Equals(Name.Text.Trim()));
                        var index = lsnvm.StudentCollection.IndexOf(itemSelected);
                        StudentNui std = lsnvm.StudentCollection[index];
                        //message = string.Format("Name: {0}\nClass: {1}\nTime: {2}", std.Id, std.Name, std.Phone);
                        //DisplayAlert("Notice", message, "OK");
                        Navigation.PushAsync(new StudentDetailPage(std));
                    }
                }
            }
            catch(Exception ex)
            {
                DisplayAlert("ERROR", ex.Message, "OK");
            }
            
        }
        public static string process_id_atd;
        #region Handle Attendances Functions()
        [Obsolete]
        private async void TakeAttendance(object sender, EventArgs e) //create process and request to server to open camera
        {
            try
            {
                if(lsnvm.StudentCollection.Count <= 0)
                {
                    await DisplayAlert("Notice", "No students in class. Import student list!", "OK");
                }
                else
                {
                    var thread = new Thread(paintLoading);
                    thread.Start();

                    //Declare api_key
                    var httpClient = new HttpClient();
                    var api_key = Data.Data.Instance.UserNui.authorization;
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("authorization", api_key);

                    var base1_URL = HomePage.base_URL + "/attendance/process/create/" + SubjectPage.classes.Name + "/";
                    var result1 = await httpClient.GetAsync(base1_URL);
                    var responseProcess = await result1.Content.ReadAsStringAsync();
                    var process = JsonConvert.DeserializeObject<Process>(responseProcess);
                    process_id_atd = process.process_id;
                    if (process.process_id!="")
                    {
                        var httpService2 = new HttpClient();
                        var api_key2 = Data.Data.Instance.UserNui.authorization;
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("authorization", api_key2);
                        var base2_URL = HomePage.base_URL + "/attendance/list/" + process.process_id + "/";
                        var result2 = await httpService2.GetAsync(base2_URL);
                        var contentAttendance = await result2.Content.ReadAsStringAsync();

                        thread.Abort();

                        var attendanceList =  JsonConvert.DeserializeObject<List<AttendanceNui>>(contentAttendance);
                        foreach(AttendanceNui atd in attendanceList)
                        {   
                            try
                            {
                                if (atd.type == "unknown")
                                {
                                    listUnknownImage.Add(atd);
                                }
                                var checkAttendance = lsnvm.StudentCollection.Single(r => r.student_id.Trim().Equals(atd.id.Trim()) && atd.type.Equals("known"));
                                checkAttendance.state = true;
                                checkAttendance.confidence = atd.confidence + "%";
                                checkAttendance.img_attendance = atd.img_face;
                                
                            }
                            catch(Exception) {

                            }

                        }

                        foreach (StudentNui std in lsnvm.StudentCollection)
                        {
                            if (std.state == false)
                            {
                                listNotYetAtd.Add(std);
                            }
                        }

                        ReLoadStudenList();
                    }

                    //PostRawListStudent();
                    //ReLoadStudenList();
                    //var base2_URL = HomePage.base_URL + "/attendance/list/" + process.id + "/";
                    //var result2 = await httpClient.GetAsync(base2_URL);
                    //var responseProcess = await result2.Content.ReadAsStringAsync();
                }
            }
            catch(Exception ex)
            {
                await DisplayAlert("Notice", ex.Message, "OK");
            }
        }

        private void ToUnknownPage(object sender, EventArgs e)
        {
            Navigation.PushAsync(new UnknownPage(listUnknownImage, listNotYetAtd));
        }

        int TimeCount = 20;
        public void paintLoading()
        {
            try
            {
                using (IProgressDialog progress = UserDialogs.Instance.Progress("Taking attendance...", null, null, true, MaskType.Gradient))
                {
                    int i = 0;
                    while(true)
                    {
                        progress.PercentComplete = i++;
                        //Task.Delay(2000);
                        // Thread.Sleep(TimeCount * 1000 / 100);
                        Thread.Sleep(130);
                    }
                }
                UserDialogs.Instance.Toast("Done");
            }
            catch (Exception)
            {
               /* Device.BeginInvokeOnMainThread(() => {
                    DisplayAlert("Notice", ex.Message, "OK");
                });*/
            }
        }

        public async void PostRawListStudent()
        {
            try
            {
                List<String> list_id = new List<string>();  // list to post to server
                foreach (Student std in ClassPage.classes.StudentList1)
                {
                    list_id.Add(std.Id.Trim());
                }
                ListID listId = new ListID("10232602020181", list_id);
                var httpService = new HttpClient();
                string jsonListId = JsonConvert.SerializeObject(listId);
                StringContent contentAttendance = new StringContent(jsonListId, Encoding.UTF8, "application/json");
                var baseAttendance_URL = @"http://192.168.30.104:9000/mobile/";

                CheckSquence++;

                if (CheckSquence == 1)
                {
                    var thread = new Thread(paintLoading);
                    thread.Start();
                    HttpResponseMessage responseAttendance = await httpService.PostAsync(baseAttendance_URL, contentAttendance);
                    var result = await responseAttendance.Content.ReadAsStringAsync();
                    thread.Abort();

                    listAttendance = JsonConvert.DeserializeObject<ObservableCollection<Attendance>>(result);
                    foreach (Attendance atd in listAttendance)
                    {
                        if (atd.state == true)
                        {
                            var student = lsvm.StudentCollection.Single(r => r.Id.Trim().Equals(atd.student_id.Trim()));
                            student.State = true;
                        }
                    }
                    UserDialogs.Instance.Toast("Done");
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Notice", "No students in class. Import student list!", "OK");
            }
        }

        public void HandleAttendance()
        {

        }
        #endregion

        #region Functions for Save Click
        [Obsolete]
        public async void HandlePutStateSchedule(Schedule schedule) //update  schedule to server
        {
            var httpService = new HttpClient();
            string jsonSchedule = JsonConvert.SerializeObject(schedule); // convert object => json
            string colorState = "," + @"""colorState""";
            int removeIndex = jsonSchedule.IndexOf(colorState);
            jsonSchedule = jsonSchedule.Substring(0, removeIndex) + "}";
            StringContent contentLecture = new StringContent(jsonSchedule, Encoding.UTF8, "application/json");
            var baseLecture_URL = HomePage.base_URL + "/schedule/" + schedule.id.ToString();
            HttpResponseMessage responseLecture =  await httpService.PutAsync(baseLecture_URL, contentLecture);
        }

        [Obsolete]
        public async void HandlePutStateRoom(Schedule schedule) //update room to server
        {
            var roomNow = HomePage._lrvm.RoomCollection.Single(r => r.room_id == schedule.idRoom);
            roomNow.state = "Available";
            var httpService = new HttpClient();
            string jsonRoom= JsonConvert.SerializeObject(roomNow); // convert object => json
            StringContent contentLecture = new StringContent(jsonRoom, Encoding.UTF8, "application/json");
            var baseLecture_URL = HomePage.base_URL + "/room/" + roomNow.room_id.ToString();
            HttpResponseMessage responseLecture = await httpService.PutAsync(baseLecture_URL, contentLecture);
        }

        [Obsolete]
        public async void HandlePutStateProcess()
        {
            //int status = 1;
            //var date = DateTime.Now.Date.ToString().Substring(0, 19);
            //var time = DateTime.Now.TimeOfDay.ToString().Substring(0, 8);

            try
            {
                var httpService = new HttpService();
                string date = JsonConvert.SerializeObject(DateTime.Today);
                date = date.Substring(1, 19);
                date = date.Replace("00:00:00", "12:00:00");
                var base_URL = HomePage.base_URL + "/process?subject_id=" + SubjectPage.classes.Name + "&date=" + date;
                var result = await httpService.SendAsync(base_URL, HttpMethod.Get);

                var process = JsonConvert.DeserializeObject<List<Process>>(result);
                if(process.Count == 1)
                {
                    var getProcess = process[0];
                    getProcess.state = true;
                    var httpClient = new HttpClient();
                    string jsonProcess = JsonConvert.SerializeObject(getProcess);
                    StringContent contentProcess = new StringContent(jsonProcess, Encoding.UTF8, "application/json");
                    var baseProcess_URL = HomePage.base_URL + "/process/" + getProcess.process_id.ToString();
                    await httpClient.PutAsync(baseProcess_URL, contentProcess);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Notice", "Fail in HandlePutStateProcess \n" + ex.Message, "OK");
            }
        }

        [Obsolete]
        private async void ClickSaveToEndClass(object sender, EventArgs e)
        {
            string subject_id;
            string name_subject;
            string timeSlot;
            int attendanceCount = 0;

            var subject = HomePage._lsjvm.SubjectCollection.Single(r => r.subject_id == SubjectPage.enableSubJectId);
            int index = HomePage._lsjvm.SubjectCollection.IndexOf(subject);

            subject_id = subject.subject_id;
            timeSlot = subject.time_slot;
            name_subject = subject.name;
            foreach (Student std in lsvm.StudentCollection)
            {
                if (std.State == true)
                {
                    attendanceCount++;
                }
            }
            var message = String.Format("Class: {0}\nSubject: {1}\nTime Slot: {2}\nAttendance: {3}", subject_id, name_subject, timeSlot, attendanceCount);
            await DisplayAlert("Class Info", message, "Continue");
            bool answer = await DisplayAlert("Notice", "You will be return to home page after save this class", "OK", "Cancel");
            if (answer)
            {
                if (index < HomePage._lsjvm.SubjectCollection.Count - 1) // nếu index của subject vẫn còn nằm trong _lsjvm
                {
                    SubjectPage.enableSubJectId = HomePage._lsjvm.SubjectCollection[index + 1].subject_id; // gán enableSubjectID = id của subject tiếp theo
                    subject.stateString = attendanceCount.ToString() + " / " + lsvm.StudentCollection.Count.ToString();
                    SubjectPage.checkClearStd_ListPage = 1; // =1 để khi back về chọn schedule mới sẽ clear list student cũ
                    //process.state = 1; // state = 1 là process,subject này done
                }
                else
                {
                    SubjectPage.enableSubJectId = "-1"; // nếu index vượt thì gán = -1 để ko làm gì khi back về
                    //process.state = 1; / tiep tuc thay doi state cho process = 1;
                    subject.stateString = attendanceCount.ToString() + " / " + lsvm.StudentCollection.Count.ToString();
                }

                // Put to Server
                //HandlePutStateProcess();
                //HandlePutStateRoom(schedule);    //cap nhat state cua Room

                await Navigation.PopAsync();
            }
            else
            {
                return;
            }
        }
        #endregion

        #region ChartPage()

        public void GetDataForPieChart()
        {
            int attendanceCount = 0;
            foreach (Student std in lsvm.StudentCollection)
            {
                if (std.State == true)
                {
                    attendanceCount++;
                }
            }
            ChartPage.attendances = attendanceCount;
            ChartPage.absentees = SubjectPage.classes.StudentList1.Count - attendanceCount;
        }


        private void ToChartPage(object sender, EventArgs e)
        {
            GetDataForPieChart();
            Navigation.PushAsync(new ChartPage());
        }
        #endregion

        [Obsolete]
        private async void ClickSaveAndImport(object sender, EventArgs e) // phai luu ve DB
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
            foreach (Student std in lsvm.StudentCollection)
            {
                if (std.State == true)
                {
                    attendanceCount++;
                }
            }

            var message = String.Format("Class: {0}\nSubject: {1}\nTime Slot: {2}\nAttendance: {3}", className, subject, timeSlot, attendanceCount);
            await DisplayAlert("Class Info", message, "Continue");
            bool answer = await DisplayAlert("Notice", "You will be return to home page after save this class", "OK", "Cancel");
            if (answer)
            {
                if (index < HomePage._lsvm.ScheduleCollection.Count - 1) // nếu index của schedule vẫn còn nằm trong _lsvm
                {
                    ClassPage.first_id_in_list = Convert.ToInt32(HomePage._lsvm.ScheduleCollection[index + 1].id); // gán first id in list = id của schedule tiếp theo
                    ClassPage.checkClearStd_ListPage = 1; // =1 để khi back về chọn schedule mới sẽ clear list student cũ
                    schedule.state = 1; // state = 1 là schdule này done
                    schedule.stateString = attendanceCount.ToString() + " / " + lsvm.StudentCollection.Count.ToString();
                }
                else
                {
                    ClassPage.first_id_in_list = -1; // nếu index vượt thì gán = -1 để ko làm gì khi back về
                    schedule.state = 1;
                    schedule.stateString = attendanceCount.ToString() + " / " + lsvm.StudentCollection.Count.ToString();
                }

                // Put to Server
                HandlePutStateSchedule(schedule); //cap nhat state cua Schedule
                HandlePutStateRoom(schedule);    //cap nhat state cua Room

                await Navigation.PopAsync();
            }
            else
            {
                return;
            }
        }

        #region ExportExcel
        private void ExportExcel(object sender, EventArgs e)
        {
            ExportExcel();
        }

        public async void ExportExcel()
        {
            try
            {
                //using (ExcelEngine excelEngine = new ExcelEngine())
                //{
                //    IApplication application = excelEngine.Excel;

                //    application.DefaultVersion = ExcelVersion.Excel2016;
                //    //Create a workbook with a worksheet
                //    IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);

                //    //Access first worksheet from the workbook instance.
                //    IWorksheet worksheet = workbook.Worksheets[0];

                //    worksheet.Range["A1:E1"].Merge();
                //    worksheet.Range["A1:E1"].Text = "Danh sách sinh viên";
                //    worksheet.Range["A2:E2"].Merge();
                //    //worksheet.Range["A2:E2"].Text = "Lớp: " + schedule.nameSubject + " (" + schedule.idSubject + " )";

                //    worksheet.Range["A4"].Text = "ID";
                //    worksheet.Range["B4"].Text = "Họ Tên";
                //    //worksheet.Range["C4"].Text = schedule.date.ToString();

                //    //for (int i = 4; i< listStudent.Count + 4; i++)
                //    //{

                //    //}

                //    MemoryStream stream = new MemoryStream();
                //    workbook.SaveAs(stream);


                //    workbook.Close();

                //    //Save the stream as a file in the device and invoke it for viewing
                //    await Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("TestExportExcel.xlsx", "application/msexcel", stream);
                //}
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
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
        #endregion

        
    }
}
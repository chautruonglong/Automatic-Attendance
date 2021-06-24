using Acr.UserDialogs;
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
        public static ListStudentNuiViewModel lsnvm = new ListStudentNuiViewModel();
        public static ObservableCollection<Attendance> listAttendance = new ObservableCollection<Attendance>();
        public List<AttendanceNui> listUnknownImage = new List<AttendanceNui>();
        public static List<StudentNui> listNotYetAtd = new List<StudentNui>();
        public static Subject subjectStatic;

        [Obsolete]
        public ListStudentPage(Subject subject)
        {

            InitializeComponent();
            subjectStatic = subject;
            this.BindingContext = new ListStudentNuiViewModel();
            ShowStudentList(subject.subject_id); //goi student list xuong'
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


        private async void AddSingleStudent(object sender, EventArgs e) // xu li khi them tung student
        {
            //var page = new PopUpView();
            //page.Action += async (sender1, stringparameter) =>
            //{

            //    if (stringparameter != null)
            //    {
                    
            //    }
            //    else
            //    {
            //        await DisplayAlert("Notice", "Invalid Syntax", "OK");
            //        return;
            //    }
            //};
            //await PopupNavigation.Instance.PushAsync(page);
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

        public static string process_id_atd = "";  //process_id tra ve khi bam' take attendance
        public static List<AttendanceNui> listAttendanceOfProcess = new List<AttendanceNui>(); // list attendance tra ve sau khi take attendance co state = true;
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

                    var base1_URL = HomePage.base_URL + "/attendance/process/create/" + subjectStatic.subject_id + "/";
                    var result1 = await httpClient.GetAsync(base1_URL);
                    var responseProcess = await result1.Content.ReadAsStringAsync();
                    var process = JsonConvert.DeserializeObject<Process>(responseProcess);
                    if (process.process_id!="")
                    {
                        process_id_atd = process.process_id;
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
                                checkAttendance.confidence = atd.confidence;
                                checkAttendance.img_attendance = atd.img_face;

                                //
                                if(atd.id == checkAttendance.student_id)
                                {
                                    listAttendanceOfProcess.Add(atd);
                                }
                            }
                            catch(Exception) {

                            }
                        }

                        ReLoadStudenList();
                    }
                }
            }
            catch(Exception ex)
            {
                await DisplayAlert("Notice", ex.Message, "OK");
            }
        }

        

        //int TimeCount = 20;
        public void paintLoading()
        {
            try
            {
                using (IProgressDialog progress = UserDialogs.Instance.Progress("Taking attendance...", null, null, true, MaskType.Gradient))
                {
                    int i = 0;
                    while (true)
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


        #endregion

        #region Functions for Save Click
        [Obsolete]

        public static List<AtdModify> listAtdModify = new List<AtdModify>();

        [Obsolete]
        public async Task<bool> SaveLastProcessWithAttendance()
        {
            if (process_id_atd != "")
            {
                var httpClient = new HttpClient();
                var api_key = Data.Data.Instance.UserNui.authorization;
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("authorization", api_key);
                List<string> listIdAttendance = new List<string>();


                foreach (StudentNui stdn in listNotYetAtd) // false -> true
                {
                    if (stdn.state == true)
                    {
                        listAtdModify.Add(new AtdModify(stdn.student_id, stdn.state));
                    }
                }


                try
                {
                    foreach (AttendanceNui atd in listAttendanceOfProcess) //true->false
                    {
                        var std = lsnvm.StudentCollection.Single(r => r.student_id == atd.id);
                        if (std.state == false)
                        {
                            listAtdModify.Add(new AtdModify(std.student_id, std.state));
                        }
                    }
                }
                catch(Exception ex)
                {
                    await DisplayAlert("ERROR", "SaveLastProcess" + ex.Message, "OK");
                }
                
                var base_URL = HomePage.base_URL + "/attendance/update/" + subjectStatic.subject_id + "/" + process_id_atd + "/";
                process_id_atd = "";
                var jsonLastAttendance = JsonConvert.SerializeObject(listAtdModify);
                StringContent content = new StringContent(jsonLastAttendance, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PutAsync(base_URL,content);
                if (response.IsSuccessStatusCode)
                {
                    await Navigation.PopAsync();
                }
                return true; // đã có process, chớ kh phải kiểm tra coi respone có work
            }
            else 
            {
                await DisplayAlert("ERROR", "You must take attendance first", "OK");
                return false;
            }
        }

        [Obsolete]
        private async void ClickSaveToEndClass(object sender, EventArgs e)
        {
            try
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
                foreach (StudentNui std in lsnvm.StudentCollection)
                {
                    if (std.state == true)
                    {
                        attendanceCount++;
                    }
                }
                var message = String.Format("Class: {0}\nSubject: {1}\nTime Slot: {2}\nAttendance: {3}", subject_id, name_subject, timeSlot, attendanceCount);
                await DisplayAlert("Class Info", message, "Continue");
                bool answer = await DisplayAlert("Notice", "You will be return to home page after save this class", "OK", "Cancel");
                if (answer)
                {
                    //I. thay doi Tren Server truoc:
                    var checkProcessCoTonTai =  await SaveLastProcessWithAttendance();

                    //II. thay doi Ttrên list sauu:
                    if (checkProcessCoTonTai)
                    {
                        if (index < HomePage._lsjvm.SubjectCollection.Count - 1) // nếu index của subject vẫn còn nằm trong _lsjvm
                        {
                            SubjectPage.enableSubJectId = HomePage._lsjvm.SubjectCollection[index + 1].subject_id; // gán enableSubjectID = id của subject tiếp theo
                            subject.stateString = attendanceCount.ToString() + " / " + lsnvm.StudentCollection.Count.ToString();
                            SubjectPage.checkClearStd_ListPage = 1; // =1 để khi back về chọn schedule mới sẽ clear list student cũ

                        }
                        else
                        {
                            SubjectPage.enableSubJectId = "-1"; // nếu index vượt thì gán = -1 để ko làm gì khi back về
                            subject.stateString = attendanceCount.ToString() + " / " + lsnvm.StudentCollection.Count.ToString();
                        }
                    }
                    
                    //await Navigation.PopAsync();
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Fail in Save to end class "+ ex.Message, "OK");
            }
            
            
        }
        #endregion

        #region ChartPage()

        public void GetDataForPieChart()
        {
            int attendanceCount = 0;
            foreach (StudentNui std in lsnvm.StudentCollection)
            {
                if (std.state == true)
                {
                    attendanceCount++;
                }
            }
            ChartPage.attendances = attendanceCount;
            ChartPage.absentees = lsnvm.StudentCollection.Count - attendanceCount;
        }


        private void ToChartPage(object sender, EventArgs e)
        {
            GetDataForPieChart();
            Navigation.PushAsync(new ChartPage());
        }
        #endregion

        private void ToUnknownPage(object sender, EventArgs e)
        {
            listNotYetAtd.Clear();  //clear listNotYetAtd moi khi bam xem unknown list
            foreach (StudentNui std in lsnvm.StudentCollection) // list absent 
            {
                if (std.state == false)
                {
                    listNotYetAtd.Add(std);
                }
            }
            Navigation.PushAsync(new UnknownPage(listUnknownImage));
        }

        [Obsolete]
       

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
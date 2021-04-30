using AutoAttendant.Models;
using AutoAttendant.ViewModel;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        ListStudentViewModel lsvm = new ListStudentViewModel();
        public ListStudentPage()
        {
            InitializeComponent();
            this.BindingContext = new ListStudentViewModel();
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

                    message = string.Format("Name: {0} \nClass: {1} \nTime: {2}", std.Id, std.Name, std.Phone);
                    DisplayAlert("Notice", message , "OK");
                    Navigation.PushAsync(new StudentDetailPage(std));
                }
            }
        }

        private void OpenPicker(object sender, EventArgs e)
        {
            PickerSort.IsEnabled = true;
            PickerSort.Focus();
        }

        private void HandlePickerSort(object sender, EventArgs e) // xu li sort option
        {
            var index = PickerSort.SelectedIndex;
            if(index != -1)
            {
                btnSortOption.Text = PickerSort.Items[index].ToString();
            }
        }
        async void ImportExcel(object sender, EventArgs e) // xu li import excel them student
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
                for (int i = 8; i <= 55; i++)
                {
                    string id = "B" + i.ToString();
                    string name = "C" + i.ToString();
                    string phone = "D" + i.ToString();
                    var mess1 = worksheet.Range[id].Text.ToString();
                    var mess2 = worksheet.Range[name].Text.ToString();
                    var mess3 = worksheet.Range[phone].Text.ToString();

                    try
                    {
                        Student student = new Student(mess1, mess2, "18TCLC-DT2", "IT", mess3, "url");
                        lsvm.StudentCollection.Add(student);
                    }
                    catch(Exception ex)
                    {
                        await DisplayAlert("Notice", ex.Message, "OK");
                    }
                }
                this.BindingContext = lsvm;
                
                //MemoryStream stream1 = new MemoryStream();
                //workbook.SaveAs(stream1);

                //workbook.Close();
                //excelEngine.Dispose();

            }
        }

        private void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            string value = string.Empty;
            CheckBox cbStatus = sender as CheckBox;
            if (cbStatus.IsChecked == true)
            {
                btn_Excel.BackgroundColor = Color.FromHex("#246CFE");
            }
            else btn_Excel.BackgroundColor = Color.FromHex("#021135");
        }

        private void AddSingleStudent(object sender, EventArgs e) // xu li khi them tung student
        {

        }
    }
}
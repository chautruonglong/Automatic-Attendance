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
        ListScheduleViewModel lsvm = new ListScheduleViewModel();

        public ClassPage()
        {
            InitializeComponent();
            this.BindingContext = new ListScheduleViewModel();
            ShowSchedule();
        }

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
                    if (firstLabel.GetType() == typeof(Label) && secondLabel.GetType() == typeof(Label) && thirdLabel.GetType() == typeof(Label))
                    {
                        Label ClassName = (Label)firstLabel;
                        Label Subject = (Label)secondLabel;
                        Label TimeSlot = (Label)thirdLabel;

                        var itemSelected = lsvm.ScheduleCollection.Single(r => r.Classes == ClassName.Text && r.Subject == Subject.Text);
                        var index = lsvm.ScheduleCollection.IndexOf(itemSelected);
                        Schedule schedule = lsvm.ScheduleCollection[index];

                        message = string.Format("Id Room: {0} \nClass: {1} \nSubject: {2} \nTime Slot: {3} \nState: {4}", schedule.IdRoom, schedule.Classes, schedule.Subject, schedule.TimeSlot, schedule.State);
                        bool answer = await DisplayAlert("Room Info", message, "Join", "Cancel");
                        if (answer)
                        {
                            await Navigation.PushAsync(new ClassTabbedPage());
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }
            catch (Exception)
            {
                
            }
            //Navigation.PushAsync(new ClassTabbedPage());
        }


        [Obsolete]
        private async void AddRoom(object sender, EventArgs e)
        {
            await PopupNavigation.PopAsync();

        }

        public async Task<ObservableCollection<Schedule>> HandleSchedule()
        {
            try
            {
                //var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                //{
                //    { DevicePlatform.iOS, new[] {"com.microsoft.xlsx"} },
                //    { DevicePlatform.Android, new[] { "application/json" } },
                //});

                //var pickerResult = await FilePicker.PickAsync(new PickOptions
                //{
                //    FileTypes = customFileType,
                //    PickerTitle = "Pick an Excel file"
                //});

                //if (pickerResult != null)
                //{
                    //var resourcePath = pickerResult.FullPath.ToString();

                    //var resourcePath = "/storage/emulated/0/Android/data/com.companyname.autoattendant/cache/2203693cc04e0be7f4f024d5f9499e13/495b0c23a318409987d641309fb9543d/NinhKhanhDuy_CNTT_30042021.json";
                    //using (StreamReader r = new StreamReader(resourcePath))
                    //{
                    //    string json = r.ReadToEnd();
                    //    var listClass = JsonConvert.DeserializeObject<List<Schedule>>(json);
                    //    return listClass;
                    //}
                //}
                //else return null;
                var httpService = new HttpService();
                string full_url = "http://100.93.173.240:3000/schedule/";
                var result = await httpService.SendAsync(full_url, HttpMethod.Get);
                var listSchedule = JsonConvert.DeserializeObject<ObservableCollection<Schedule>>(result);
                return listSchedule;
            }
            catch (Exception)
            {
                await DisplayAlert("Notice", "Fail", "OK");
                
                return null;
            }
        }

        public async void ShowSchedule()
        {
            try
            {
                var listSchedule = new ObservableCollection<Schedule>(await HandleSchedule()); // list Schedule 

                foreach (Schedule schedule in listSchedule)
                {
                    lsvm.ScheduleCollection.Add(schedule);
                }
                this.BindingContext = lsvm;
            }
            catch (Exception)
            {
                await DisplayAlert("Error", "Can not get Schedule", "OK");
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
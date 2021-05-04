using AutoAttendant.Models;
using AutoAttendant.ViewModel;
using AutoAttendant.Views.PopUp;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
        ListClassViewModel lcvm = new ListClassViewModel();
        public ClassPage()
        {
            InitializeComponent();
            this.BindingContext = new ListClassViewModel();
        }

        private void ClassClick(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ClassTabbedPage());
        }


        [Obsolete]
        private async void AddRoom(object sender, EventArgs e)
        {
            await PopupNavigation.PopAsync();

        }

        public async Task< List<Schedule> > HandleSchedule()
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
                    var resourcePath = "/storage/emulated/0/Android/data/com.companyname.autoattendant/cache/2203693cc04e0be7f4f024d5f9499e13/495b0c23a318409987d641309fb9543d/NinhKhanhDuy_CNTT_30042021.json";
                    using (StreamReader r = new StreamReader(resourcePath))
                    {
                        string json = r.ReadToEnd();
                        var listClass = JsonConvert.DeserializeObject<List<Schedule>>(json);
                        return listClass;
                    }
                //}
                //else return null;
            }
            catch (Exception)
            {
                await DisplayAlert("Notice", "Fail", "OK");
                return null;
            }
        }

        [Obsolete]
        private async void ShowListClass(object sender, EventArgs e) // 
        {
            string className = String.Empty;
            string subject = String.Empty;
            string timeSlot = String.Empty;

            var listClass = new ObservableCollection<Schedule>(await HandleSchedule()); // list Schedule 

            foreach(Schedule classs in listClass)
            {
                className = classs.Classes;
                subject = classs.Subject;
                timeSlot = classs.TimeSlot;
                var itemClass = className + "-" + subject + "- [ " + timeSlot + " ]";
                PickerClass.Items.Add(itemClass);
            }
            PickerClass.IsEnabled = true;
            PickerClass.Focus();
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

        private void DeleteRoom(object sender, EventArgs e)
        {
            Image img = sender as Image;
            var stackLayout = img.Parent;
            var checkStack = stackLayout.GetType();
            if (checkStack == typeof(StackLayout))
            {
                StackLayout container = (StackLayout)stackLayout;
                var listChild = container.Children;

                var lb_room = listChild[0].GetType();
                if (lb_room == typeof(Label))
                {
                    Label lb = (Label)listChild[0];

                    var itemToRemove = lcvm.ClassCollection.Single(r => r.Name == lb.Text);
                    lcvm.ClassCollection.Remove(itemToRemove);
                }
                else
                {
                    DisplayAlert("Fail", "Fail", "Try Again");
                }
            }
        }

        private void HandlePickerClass(object sender, EventArgs e)
        {
            var index = PickerClass.SelectedIndex;
            if (index != -1)
            {
                //var message = PickerClass.Items[index].ToString();
                //DisplayAlert("Ranh cn", message, "OK");
                Navigation.PushAsync(new ListStudentPage());
            }
        }
    }
}
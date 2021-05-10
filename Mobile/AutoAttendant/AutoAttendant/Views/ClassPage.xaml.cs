﻿using AutoAttendant.Models;
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
        ListScheduleViewModel lsvm = LoginPage._lsvm;
        public static ListClassViewModel _lcvm = new ListClassViewModel();
        public static Classes classes = new Classes();
        public static int checkClearStd_ListPage = 0;
        public static int first_id_in_list; // id dau tien cua list schedule
        public ClassPage()
        {
            InitializeComponent();
            this.BindingContext = new ListScheduleViewModel();
            ShowSchedule();
            
        }



        protected override void OnAppearing() // goi khi back ve`
        {
            ReLoadScheduleList();
            base.OnAppearing();
        }

        public void ReLoadScheduleList()
        {
            //SetColorById();
            lsvm.ScheduleCollection= LoginPage._lsvm.ScheduleCollection;
            if (lsvm.ScheduleCollection.Count > 0)
            {
                
                this.BindingContext = new ListScheduleViewModel();
                this.BindingContext = lsvm;
                SetColorById();

            }
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
                    var forthLabel = listChildren[3];
                    if (firstLabel.GetType() == typeof(Label) && secondLabel.GetType() == typeof(Label) && thirdLabel.GetType() == typeof(Label))
                    {
                        Label ClassName = (Label)firstLabel;
                        Label Subject = (Label)secondLabel;
                        Label TimeSlot = (Label)thirdLabel;
                        Label LabelId = (Label)forthLabel;
                        if(Convert.ToInt32(LabelId.Text) == first_id_in_list)
                        {
                            var itemSelected = lsvm.ScheduleCollection.Single(r => r.Classes == ClassName.Text && r.Subject == Subject.Text);
                            var index = lsvm.ScheduleCollection.IndexOf(itemSelected);
                            Schedule schedule = lsvm.ScheduleCollection[index];

                            message = string.Format("Id Room: {0} \nClass: {1} \nSubject: {2} \nTime Slot: {3} \nState: {4}", schedule.IdRoom, schedule.Classes, schedule.Subject, schedule.TimeSlot, schedule.State);
                            bool answer = await DisplayAlert("Room Info", message, "Join", "Cancel");
                            if (answer)
                            {
                                classes.Name = schedule.Classes;
                                //classes.StudentList1.Clear();
                                if(checkClearStd_ListPage == 1)
                                {
                                    classes.StudentList1.Clear();
                                    checkClearStd_ListPage = 0;
                                }
                                await Navigation.PushAsync(new ClassTabbedPage(classes));
                            }
                            else
                            {
                                return;
                            }

                        }
                        else
                        {
                            await DisplayAlert("Notice","Your class must be finished!","OK");
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

                var httpService = new HttpService();
                string full_url = "http://192.168.0.101:3000/schedule/";
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


        public void SetColorById()
        {   
            if (first_id_in_list!=-1) {
                var item = LoginPage._lsvm.ScheduleCollection.Single(r => r.Id == first_id_in_list);
                item.ColorState = "#ccc";
                int index = LoginPage._lsvm.ScheduleCollection.IndexOf(item);
                if (index > 0) { LoginPage._lsvm.ScheduleCollection[index - 1].ColorState = "#246CFE"; }
                
            }
            else
            {
                var item = LoginPage._lsvm.ScheduleCollection[LoginPage._lsvm.ScheduleCollection.Count - 1];
                item.ColorState = "#246CFE";
            }




        }
        public async void ShowSchedule()
        {
            try
            {   if (LoginPage.checkCreateListSchedule == 0) {
                    var listSchedule = new ObservableCollection<Schedule>(await HandleSchedule()); // list Schedule }
                    first_id_in_list = Convert.ToInt32(listSchedule[0].Id);

                    foreach (Schedule schedule in listSchedule)
                    {
                        LoginPage._lsvm.ScheduleCollection.Add(schedule);
                    }
                    LoginPage.checkCreateListSchedule = 1;
                }




                SetColorById();
                lsvm.ScheduleCollection = LoginPage._lsvm.ScheduleCollection; // avoid add same schedule
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
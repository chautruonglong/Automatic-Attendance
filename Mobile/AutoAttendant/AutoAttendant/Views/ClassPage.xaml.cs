using AutoAttendant.Models;
using AutoAttendant.ViewModel;
using AutoAttendant.Views.PopUp;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        [Obsolete]
        private async void ShowPopUpAddClass(object sender, EventArgs e) // Show Popup and handle data from popup
        {
            string className = String.Empty;
            string message = String.Empty;
            var page = new PopUpAddClass();
            page.Action += async (sender1, stringparameter) =>
            {
                
                if(stringparameter != null)
                {
                    className = stringparameter; // get data tu` PopUp
                    className = className.Replace(" ", "");
                    className = className.ToUpper();
                    message = className + " was added successfully";

                    Classes classs = new Classes(className);
                    lcvm.ClassCollection.Add(classs);
                    this.BindingContext = lcvm;


                    await DisplayAlert("Notice", message, "OK");

                }
                else
                {
                    await DisplayAlert("Notice", "Invalid Syntax" , "OK");
                    return;
                }
            };

            page.Disappearing += (c, d) =>
            {
                if (className != null)
                {
                    
                }
            };
            await PopupNavigation.Instance.PushAsync(page);
            //PopupNavigation.PushAsync(new PopUpView());
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

    }
}
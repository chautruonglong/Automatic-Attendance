using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace AutoAttendant.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : MasterDetailPage
    {
        public HomePage()
        {
            InitializeComponent();
            Detail = new NavigationPage(new ClassPage());
            
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            // Ko dung`
        }

        private void HandleTeacherProfile(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new TeacherInfoPage());
            IsPresented = false;
        }

        private void HandleSetting(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new SettingPage());
            IsPresented = false;
        }

        private void HandleLogOut(object sender, EventArgs e)
        {
            Navigation.PopToRootAsync();
        }


        private void HandleHome(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new ClassPage());
            Detail = new NavigationPage(new ClassPage());
            IsPresented = false;
        }

        private void ChangeAvatar(object sender, EventArgs e)
        {
            CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                DisplayAlert("Notice", "Picking a photo is not supported", "OK");
                return;
            }
            var mediaOptions = new PickMediaOptions()
            {
                PhotoSize = PhotoSize.Medium
            };
            var selectedFile = CrossMedia.Current.PickPhotoAsync(mediaOptions);
            if (selectedFile == null)
            {
                DisplayAlert("Error", "Could not get the image from Gallery!", "OK");
                Avatar.Source = "DefaultAvatar.jpg";
            }
            //Avatar.Source = ImageSource.FromStream(() => selectedFile.GetStream());
            else
            {
                Avatar.Source = ImageSource.FromStream(() => selectedFile.Result.GetStream());
            }
            

        }
    }
}
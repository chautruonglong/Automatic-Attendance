using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoAttendant.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {
        public Page1()
        {
            InitializeComponent();
        }

        async void OpenFile(object sender, EventArgs e)
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
                PickerTitle = "Pick an Image"
            });

            if (pickerResult != null)
            {
                var stream = await pickerResult.OpenReadAsync();
                resultImg.Source = ImageSource.FromStream(() => stream);
                var x = pickerResult.FullPath.ToString();
                lb_test.Text = x;

            }
        }
        //Test Git
        //Test Git
        //Test Git
        //Test Git
        async void MultiplePick(object sender, EventArgs e)
        {
            var pickerResult = await FilePicker.PickMultipleAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Images,
                PickerTitle = "Pick Image(s)"
            });

            if (pickerResult != null)
            {
                var imgList = new List<ImageSource>();
                foreach (var img in pickerResult)
                {
                    var stream = await img.OpenReadAsync();
                    imgList.Add(ImageSource.FromStream(() => stream));
                }

                collectionView.ItemsSource = imgList;

            }
        }
    }
}
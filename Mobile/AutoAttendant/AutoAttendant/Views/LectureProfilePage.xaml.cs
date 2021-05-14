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
    public partial class LectureProfilePage : ContentPage
    {
        public LectureProfilePage()
        {
            InitializeComponent();
            this.BindingContext = Data.Data.Instance.Lecture;
        }
    }
}
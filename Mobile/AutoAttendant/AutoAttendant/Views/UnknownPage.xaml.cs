using AutoAttendant.Models;
using AutoAttendant.Services;
using AutoAttendant.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoAttendant.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UnknownPage : ContentPage
    {
        ListUnknownViewModel luvm = new ListUnknownViewModel();
        public UnknownPage(List<AttendanceNui> unknownList, List<StudentNui> notYetAtdList)
        {
            InitializeComponent();
            //this.BindingContext = new ListUnknownViewModel();
            list_3x4.ItemsSource = notYetAtdList;
            list_Unknown.ItemsSource = unknownList;
        }
    }
}
using AutoAttendant.Models;
using AutoAttendant.ViewModel;
using Syncfusion.SfDataGrid.XForms;
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
    public partial class DetailHistoryPage : ContentPage
    {
        public DetailHistoryViewModel dhvm = new DetailHistoryViewModel();
        public ObservableCollection<DetailHistory> listDetailHistory = new ObservableCollection<DetailHistory>();
        public DetailHistoryPage()
        {
            InitializeComponent();
            listDetailHistory.Add(new DetailHistory("102180173", "Tran Chi Minh", "Vang", "Vang","Co","Co","Co","Vang"));
            listDetailHistory.Add(new DetailHistory("102180173", "Tran Chi Minh", "Vang", "Vang", "Co", "Co", "Co", "Vang"));
            listDetailHistory.Add(new DetailHistory("102180173", "Tran Chi Minh", "Vang", "Vang", "Co", "Co", "Co", "Vang"));
            listDetailHistory.Add(new DetailHistory("102180173", "Tran Chi Minh", "Vang", "Vang", "Co", "Co", "Co", "Vang"));
            listDetailHistory.Add(new DetailHistory("102180173", "Tran Chi Minh", "Vang", "Vang", "Co", "Co", "Co", "Vang"));
            listDetailHistory.Add(new DetailHistory("102180173", "Tran Chi Minh", "Vang", "Vang", "Co", "Co", "Co", "Vang"));

            DataGridView.ItemsSource = listDetailHistory;
            GridTextColumn orderIdColumn = new GridTextColumn();
            orderIdColumn.MappingName = "Id";
            orderIdColumn.HeaderText = "Dit mm";
            DataGridView.Columns.Add(orderIdColumn);
        }
    }
}
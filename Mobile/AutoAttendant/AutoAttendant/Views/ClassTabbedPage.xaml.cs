﻿using AutoAttendant.Models;
using AutoAttendant.ViewModel;
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
    public partial class ClassTabbedPage : TabbedPage
    {
        public static ListStudentViewModel _lsvm = new ListStudentViewModel();
        public static string nameClass;

        public ClassTabbedPage(Classes classes)
        {
            InitializeComponent();
            _lsvm.StudentCollection = classes.StudentList1;
            TabClass.Title = classes.Name;
            nameClass = classes.Name;
        }
    }
}
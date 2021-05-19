using Microcharts;
using SkiaSharp;
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
    public partial class ChartPage : ContentPage
    {
        public static int attendances;
        public static int absentees;

        private readonly ChartEntry[] entries = new[]
        {
            new ChartEntry(attendances)
            {
                Label = "Attendances",
                ValueLabel = attendances.ToString(),
                Color = SKColor.Parse("#1832F6"),
                TextColor = SKColor.Parse("#FFF"),
                ValueLabelColor = SKColor.Parse("#1832F6"),
            },
            new ChartEntry(absentees)
            {
                Label = "Absentees",
                ValueLabel = absentees.ToString(),
                Color = SKColor.Parse("#EC3108"),
                TextColor = SKColor.Parse("#FFF"),
                ValueLabelColor = SKColor.Parse("#EC3108"),           
            }
        };
        public ChartPage()
        {
            InitializeComponent();
            chartViewPie.Chart = new DonutChart {
                Entries = entries,
                LabelTextSize = 35,
                LabelMode = LabelMode.LeftAndRight,
                LabelColor = SKColor.Parse("#FFF"),
                BackgroundColor = SKColor.Parse("#021135"),
                GraphPosition = GraphPosition.Center,
                IsAnimated = true,
                AnimationProgress = 2500

            };
        }
    }
}
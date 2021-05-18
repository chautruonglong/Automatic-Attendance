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
        private readonly ChartEntry[] entries = new[]
        {
            new ChartEntry(38)
            {
                Label = "Attendances",
                ValueLabel = "38",
                Color = SKColor.Parse("#1832F6"),
                TextColor = SKColor.Parse("#FFF"),
                ValueLabelColor = SKColor.Parse("#1832F6"),
            },
            new ChartEntry(5)
            {
                Label = "Absentees",
                ValueLabel = "5",
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
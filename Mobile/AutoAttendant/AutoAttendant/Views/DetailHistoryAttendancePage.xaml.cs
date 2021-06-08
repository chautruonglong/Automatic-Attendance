using Newtonsoft.Json;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoAttendant.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailHistoryAttendancePage : ContentPage
    {
        public string subjectId;
        public DetailHistoryAttendancePage(string subject_id)
        {
            InitializeComponent();
            subjectId = subject_id;
        }

        [Obsolete]
        private async void OpenPDF(object sender, EventArgs e)
        {
            try
            {
                var httpService = new HttpClient();
                var api_key = Data.Data.Instance.UserNui.authorization;
                httpService.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("authorization", api_key);
                var base_URL = HomePage.base_URL + "/attendance/history/subject/" + subjectId + "/";
                var result = await httpService.GetAsync(base_URL);

                if (result.IsSuccessStatusCode)
                {
                    var res = await result.Content.ReadAsStringAsync();
                    var uriPDF = JsonConvert.DeserializeObject<string>(res);
                    //string uri = "http://192.168.30.107:8000/resources/statistics/excel/102324020201814/102324020201814_07-06-2021_18-05-48.pdf";
                    //string uri = "http://192.168.30.103:8000/resources/report.pdf";
                    await Browser.OpenAsync(uriPDF, BrowserLaunchMode.SystemPreferred);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("ERROR", "Fail in Open PDF " + ex.Message, "OK");
            }
        } 

        [Obsolete]
        private async void SendToEmail(object sender, EventArgs e)
        {
            try
            {
                var httpService = new HttpClient();
                var api_key = Data.Data.Instance.UserNui.authorization;
                var email = Data.Data.Instance.User.email;
                httpService.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("authorization", api_key);
                var base_URL = HomePage.base_URL + "/";
                var result = await httpService.GetAsync(base_URL);
                if (result.IsSuccessStatusCode)
                {
                    await DisplayAlert("Notice", "Please check your email", "OK");
                }
            }
            catch(Exception ex)
            {
                await DisplayAlert("ERROR", "Fail in Send to Email " + ex.Message, "OK");
            } 
        }

        #region TempGrid
        //public void ShowExcel(List<string>listId, List<string>listName)
        //{
        //    gridLayout.RowDefinitions = new RowDefinitionCollection();
        //    gridLayout.ColumnDefinitions = new ColumnDefinitionCollection();
        //    gridLayout.Children.Add(new Label { Text = "Id", TextColor = Color.Yellow }, 0, 0);
        //    gridLayout.Children.Add(new Label { Text = "Name", TextColor = Color.Yellow }, 1, 0);
        //    gridLayout.VerticalOptions = LayoutOptions.End;
        //    gridLayout.HorizontalOptions = LayoutOptions.End;
        //    for (int i = 1; i <= listId.Count; i++)
        //    {

        //        gridLayout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        //        gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        //        gridLayout.Children.Add(new Label { Text = listId[i - 1], TextColor = Color.White }, 0, i);
        //        gridLayout.Children.Add(new Label { Text = listName[i - 1], TextColor = Color.White }, 1, i);
        //        gridLayout.Children.Add(new CheckBox { IsChecked = true, },2, i);
        //    }
        //}


        public async void GetExcel()
        {
            //try
            //{
            //    var httpService = new HttpClient();
            //    //var api_key = Data.Data.Instance.UserNui.authorization;
            //    //httpService.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("authorization", api_key);
            //    //var base_URL = @"https://drive.google.com/u/0/uc?id=1ra28UsQk0Y9bDumsqXwpbXttG4eSkwky&export=download";
            //    var base_URL = @"http://192.168.30.107:8000/resources/statistics/excel/102324020201814/102324020201814_06-06-2021_17-07-37.xlsx";
            //    var result = await httpService.GetAsync(base_URL);
            //    var streamExcel = await result.Content.ReadAsStreamAsync();

            //    ExcelEngine excelEngine = new ExcelEngine();
            //    IApplication application = excelEngine.Excel;
            //    application.DefaultVersion = ExcelVersion.Excel2016;

            //    //Open the workbook
            //    IWorkbook workbook = application.Workbooks.Open(streamExcel);
            //    //Access first worksheet from the workbook.
            //    IWorksheet worksheet = workbook.Worksheets[0];
            //    var row = worksheet.Rows.Length;
            //    for (int i = 2; i <= row; i++)
            //    {
            //        string id = "A" + i.ToString();
            //        string name = "B" + i.ToString();
            //        var std_id = worksheet.Range[id].Text.ToString().Trim();
            //        var std_name = worksheet.Range[name].Text.ToString().Trim();
            //        //ListId.Add(std_id);
            //        //ListName.Add(std_name);
            //    }
            //    int j = 67;

            //while (true)
            //{
            //    string id = ((char)j).ToString();
            //    string x = id + "1";
            //    var title_id = worksheet.Range[x].DateTime;
            //    if (!title_id.ToString().Equals(""))
            //    {
            //        List<bool> list_atd = new List<bool>();
            //        for (int i = 2; i <= row; i++)
            //        {
            //            string id_value = id + i.ToString();
            //            var std_id_value = worksheet.Range[id_value].Boolean;

            //            //list_atd.Add(std_id_value);
            //        }
            //    }
            //    else
            //    {
            //        break;
            //    }
            //}

            // ShowExcel(ListId, ListName);


            //}
            //catch (Exception ex)
            //{
            //    await DisplayAlert("ERROR", ex.Message, "OK");
            //}

        }
        #endregion

        
    }
}
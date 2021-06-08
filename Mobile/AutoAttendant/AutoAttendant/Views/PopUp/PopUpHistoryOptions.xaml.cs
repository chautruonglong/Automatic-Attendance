using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoAttendant.Views.PopUp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopUpHistoryOptions
    {
        public PopUpHistoryOptions()
        {
            InitializeComponent();
        }

        public EventHandler<string> Action;

        [Obsolete]
        private  void OpenPDF(object sender, EventArgs e)
        {
            try
            {
                //var httpService = new HttpClient();
                //var api_key = Data.Data.Instance.UserNui.authorization;
                //httpService.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("authorization", api_key);
                //var base_URL = HomePage.base_URL + "/attendance/history/subject/" + subjectId + "/";
                //var result = await httpService.GetAsync(base_URL);
                //var res = await result.Conte nt.ReadAsStringAsync();
                //if (result.IsSuccessStatusCode)
                //{
                //    
                //    var uriPDF = JsonConvert.DeserializeObject<string>(res);
                //string uri = "http://192.168.30.107:8000/resources/statistics/excel/102324020201814/102324020201814_07-06-2021_18-05-48.pdf";
                string uriPDF = "http://192.168.30.103:8000/resources/report.pdf";
                     Browser.OpenAsync(uriPDF, BrowserLaunchMode.SystemPreferred);
               // }
            }
            catch (Exception ex)
            {
                 DisplayAlert("ERROR", "Fail in Open PDF " + ex.Message, "OK");
            }
            //Action?.Invoke(this, "ViewPDF");
            //await PopupNavigation.Instance.PopAsync();
        }

        private void CancelPopup(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AutoAttendant.Services
{
    public class HttpService
    {
        public async Task<string> SendAsync(string url, HttpMethod method, HttpContent content = null)
        {
            var request = new HttpRequestMessage(method, url);
            if (content != null)
            {
                request.Content = content;
            }
            using (var client = new HttpClient())
            {
                var response = await client.SendAsync(request);


                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    //Read body as Json
                    return body;
                }
                else return null;
            }
        }

        private HttpClient DefaultHttpClient()
        {
            return new HttpClient();
        }
    }
}

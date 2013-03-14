using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AllanXingCommonLibrary
{
    public abstract class GetFeedsBase : BindableBase
    {
        private HttpClient _httpclient = new HttpClient();

        public async Task GetResource(string Feeds = null)
        {
            StringBuilder result = Feeds == null ? new StringBuilder(await _httpclient.GetStringAsync(GetUrl())) : new StringBuilder(Feeds);
            result = result.Replace("\n", string.Empty);
            Parse(result.ToString());
        }

        public string PostContent_String { get; set; }

        protected virtual string GetPostContent_String()
        {
            return PostContent_String;
        }
        protected void SetReferrer(string referer)
        {
            _httpclient.DefaultRequestHeaders.Referrer = new Uri(referer);
        }
        protected string ContentType = "application/json";
        public async Task PostString()
        {
            HttpContent httpcontent = new StringContent(GetPostContent_String());
            httpcontent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(ContentType);
            HttpResponseMessage response = await _httpclient.PostAsync(GetUrl(), httpcontent);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                result = result.Replace("\n", string.Empty);
                Parse(result);
            }
        }
        protected abstract void Parse(string feed);

        protected abstract string GetUrl();
    }

}

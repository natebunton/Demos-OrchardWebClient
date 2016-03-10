using Demos.Orchard.WebClient.Models;
using Newtonsoft.Json;
using System.Configuration;
using System.IO;
using System.Net;

namespace Demos.Orchard.WebClient.Helpers
{
    public static class CmsHelper
    {
        public static Media GetMedia(string mediaUrl)
        {
            Media media = null;
            var cmsMediaUrl = ConfigurationManager.AppSettings["CmsMediaUrl"];
            var uri = string.Format(cmsMediaUrl, mediaUrl);

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.Method = "GET";
            httpWebRequest.ContentType = "application/json";

            HttpWebResponse httpWebResponse = null;

            try
            {
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                var responseStream = httpWebResponse.GetResponseStream();
                var json = ReadResponseBody(responseStream);

                media = JsonConvert.DeserializeObject<Media>(json);
            }
            catch (WebException ex)
            {
                httpWebResponse = (HttpWebResponse)ex.Response;
            }
            finally
            {

            }

            return media;
        }

        private static string ReadResponseBody(Stream stream = null)
        {
            if (stream == null)
            {
                return null;
            }

            using (var sr = new StreamReader(stream))
            {
                return sr.ReadToEnd();
            }
        }
    }
}
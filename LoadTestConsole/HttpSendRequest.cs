using System;
using System.Net;

namespace LoadTestConsole
{
    public class HttpSendRequest
    {
        public static void SendData(string url, string method)
        {
            WebResponse response = null;
            try
            {
                //NetworkCredential nc = new NetworkCredential();

                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;

                request.Method = method;

                DateTime startTime = DateTime.Now;
                // Get the response.
                response = request.GetResponse();
                DateTime endtime = DateTime.Now;

                // Display the status.
                if ((response as HttpWebResponse).StatusCode == HttpStatusCode.OK)
                {
                }
                Console.WriteLine("Status code, {0}", (response as HttpWebResponse).StatusCode.ToString());

                response.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception while sending request, {0}", e.Message);
            }
        }
    }
}

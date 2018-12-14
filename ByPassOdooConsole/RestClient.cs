using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace ByPassOdooConsole
{
    public class SendResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public string Result { get; set; }
        public string SetCookie { get; set; }
        public string ReponseUri { get; set; }
        public string Location { get; set; }
    }

    public static class RestClient
    {
        private const string browserUserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:39.0) Gecko/20100101 Firefox/39.0";
        private const string browseAccept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
        private const string browseAcceptEncoding = "gzip, deflate";
        private const string browseAcceptLanguage = "en-US,en;q=0.5";

        public static SendResponse Send(string requestUri, string method, CookieContainer cookieContainer, 
            bool isDecompression, byte[] data, string referer="")
        {
            //ServicePointManager.ServerCertificateValidationCallback =
            //    delegate (Object obj, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            //    {
            //        return true;
            //    };

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                              | SecurityProtocolType.Tls11
                                              | SecurityProtocolType.Tls12
                                              | SecurityProtocolType.Ssl3;

            HttpWebRequest request;
            HttpWebResponse response = null;
            SendResponse sendResponse = new SendResponse();
            try
            {
                request = WebRequest.Create(requestUri) as HttpWebRequest;
                //request.ProtocolVersion
                request.Method = method;
                request.UserAgent = browserUserAgent;

                request.CookieContainer = cookieContainer;
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.102 Safari/537.36";
                request.Headers.Add(HttpRequestHeader.AcceptLanguage,"en,vi;q=0.9,en-GB;q=0.8");
                request.ContentType = "application/x-www-form-urlencoded";
                //if (headerParams!=null)
                //{
                //    foreach (var headerParam in headerParams)
                //    {
                //        request.Headers.Add(headerParam.Key, headerParam.Value);
                //    }
                //}
                if (!string.IsNullOrEmpty(referer))
                {
                    request.Referer = referer;
                }
                if (isDecompression)
                {
                    request.AutomaticDecompression = (System.Net.DecompressionMethods.GZip |
                                                      System.Net.DecompressionMethods.Deflate);

                }

                if (data != null)
                {
                    // Set the content length in the request headers
                    request.ContentLength = data.Length;
                    // Write data  
                    using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }

                // Get response  
                response = request.GetResponse() as HttpWebResponse;

                if (request.HaveResponse == true && response != null)
                {
                    // Get the response stream
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        sendResponse.StatusCode = response.StatusCode;
                        sendResponse.StatusDescription = response.StatusDescription;
                        sendResponse.Result = reader.ReadToEnd();
                    }
                }
            }
            catch (WebException wex)
            {
                // This exception will be raised if the server didn't return 200 - OK  
                // Try to retrieve more information about the network error  
                if (wex.Response != null)
                {
                    using (HttpWebResponse errorResponse = (HttpWebResponse)wex.Response)
                    {
                        sendResponse.StatusCode = errorResponse.StatusCode;
                        sendResponse.StatusDescription = errorResponse.StatusDescription;
                    }
                }
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }

            return sendResponse;
        }
        public static SendResponse SendRpc(string requestUri, string method, CookieContainer cookieContainer,
            bool isDecompression, byte[] data, string referer = "")
        {
            //ServicePointManager.ServerCertificateValidationCallback =
            //    delegate (Object obj, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            //    {
            //        return true;
            //    };

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                              | SecurityProtocolType.Tls11
                                              | SecurityProtocolType.Tls12
                                              | SecurityProtocolType.Ssl3;

            HttpWebRequest request;
            HttpWebResponse response = null;
            SendResponse sendResponse = new SendResponse();
            try
            {
                request = WebRequest.Create(requestUri) as HttpWebRequest;
                //request.ProtocolVersion
                request.Method = method;
                request.UserAgent = browserUserAgent;

                request.CookieContainer = cookieContainer;
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.102 Safari/537.36";
                request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en,vi;q=0.9,en-GB;q=0.8");
                request.ContentType = "application/json-rpc";
                
                //if (headerParams!=null)
                //{
                //    foreach (var headerParam in headerParams)
                //    {
                //        request.Headers.Add(headerParam.Key, headerParam.Value);
                //    }
                //}
                if (!string.IsNullOrEmpty(referer))
                {
                    request.Referer = referer;
                }
                if (isDecompression)
                {
                    request.AutomaticDecompression = (System.Net.DecompressionMethods.GZip |
                                                      System.Net.DecompressionMethods.Deflate);

                }

                if (data != null)
                {
                    // Set the content length in the request headers
                    request.ContentLength = data.Length;
                    // Write data  
                    using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }

                // Get response  
                response = request.GetResponse() as HttpWebResponse;

                if (request.HaveResponse == true && response != null)
                {
                    // Get the response stream
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        sendResponse.StatusCode = response.StatusCode;
                        sendResponse.StatusDescription = response.StatusDescription;
                        sendResponse.Result = reader.ReadToEnd();
                    }
                }
            }
            catch (WebException wex)
            {
                // This exception will be raised if the server didn't return 200 - OK  
                // Try to retrieve more information about the network error  
                if (wex.Response != null)
                {
                    using (HttpWebResponse errorResponse = (HttpWebResponse)wex.Response)
                    {
                        sendResponse.StatusCode = errorResponse.StatusCode;
                        sendResponse.StatusDescription = errorResponse.StatusDescription;
                    }
                }
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }

            return sendResponse;
        }
    }
}

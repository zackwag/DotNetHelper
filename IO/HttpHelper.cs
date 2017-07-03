using Helper.Extensions;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Web;

namespace Helper.IO
{
    public static class HttpHelper
    {
        public static void TransmitFile(ContentType contentType, byte[] buffer, string outputFileName)
        {
            var contentDisposition = new ContentDisposition {FileName = outputFileName, Inline = false};

            TransmitFile(contentType, buffer, contentDisposition);
        }

        public static void TransmitFile(ContentType contentType, string filePath, string outputFileName)
        {
            TransmitFile(contentType, FileHelper.SaveToMemory(filePath), outputFileName);
        }

        public static void TransmitFile(ContentType contentType, string filePath, ContentDisposition contentDisposition)
        {
            TransmitFile(contentType, FileHelper.SaveToMemory(filePath), contentDisposition);
        }

        public static void TransmitFile(ContentType contentType, byte[] buffer, ContentDisposition contentDisposition)
        {
            try
            {
                var response = HttpContext.Current.Response;
                var instance = HttpContext.Current.ApplicationInstance;

                response.Clear();
                response.AddHeader("content-disposition", contentDisposition.ToString());
                response.AddHeader("content-length", buffer.Length.ToString());
                response.ContentType = contentType.HasValue() ? contentType.MediaType : FileHelper.GetMimeType(contentDisposition.FileName.GetFileExtension());
                response.BinaryWrite(buffer);

                response.Flush(); // Sends all currently buffered output to the client.
                response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.

                instance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
            }
            catch (Exception ex)
            {
                throw new Exception("error on TransmitFile method, message: " + ex.Message);
            }
        }

        public static HttpWebRequest GenerateRequest(HttpMethod method, string contentType, string uri, WebHeaderCollection headers, byte[] content)
        {
            return GenerateRequest(method.Method, contentType, uri, headers, content);
        }

        public static HttpWebRequest GenerateRequest(string method, string contentType, string uri, WebHeaderCollection headers, byte[] content)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(uri);

                request.Method = method.ToUpper();

                if (!string.IsNullOrEmpty(contentType))
                    request.ContentType = contentType;

                if (headers.HasItems())
                    request.Headers = headers;

                if (content.IsNull() || content.IsDefault()) return request;
                request.ContentLength = content.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(content, 0, content.Length);
                    stream.Close();
                }

                return request;
            }
            catch (Exception ex)
            {
                throw new Exception("error on GenerateRequest method, message: " + ex.Message);
            }
        }

        public static RequestResponse GetResponse(HttpWebRequest request)
        {
            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    return new RequestResponse
                    {
                        StatusCode = response.StatusCode,
                        Headers = response.Headers,
                        Content = FileHelper.SaveToMemory(response.GetResponseStream()),
                        Uri = response.ResponseUri
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error on GetResponse method, message: " + ex.Message);
            }
        }
    }
}

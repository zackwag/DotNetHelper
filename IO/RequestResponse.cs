using System;
using System.Net;

namespace Helper.IO
{
    public class RequestResponse
    {
        public HttpStatusCode StatusCode { get; set; }

        public WebHeaderCollection Headers { get; set; }

        public byte[] Content { get; set; }

        public Uri Uri { get; set; }
    }
}

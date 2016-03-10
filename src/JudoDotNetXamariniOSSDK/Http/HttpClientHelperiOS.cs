using System;
using System.Net.Http;
using JudoDotNetXamarin;
using Foundation;
using ModernHttpClient;

namespace JudoDotNetXamariniOSSDK
{
    public class HttpClientHelper : IHttpClientHelper
    {
        private HttpMessageHandler handler;

        public HttpMessageHandler MessageHandler {
            get { return handler ?? (handler = new NativeMessageHandler ()); }
        }
    }
}


using System;
using JudoDotNetXamarin;
using System.Net.Http;
using ModernHttpClient;

namespace JudoDotNetXamarinAndroidSDK
{
    public class HttpClientHelper : IHttpClientHelper
    {
        private HttpMessageHandler handler;

        public HttpMessageHandler MessageHandler {
            get { return handler ?? (handler = new NativeMessageHandler ()); }
        }
    }
}


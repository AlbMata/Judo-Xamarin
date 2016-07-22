using System.Net.Http;
using ModernHttpClient;

namespace JudoDotNetXamarin.Tests
{
    public class HttpClientHelper : IHttpClientHelper
    {
        private HttpMessageHandler handler;

        public HttpMessageHandler MessageHandler
        {
            get { return handler ?? (handler = new NativeMessageHandler()); }
        }
    }
}
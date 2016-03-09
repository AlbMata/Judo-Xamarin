using System;
using System.Net.Http;
using JudoDotNetXamarin;

namespace JudoDotNetXamariniOSSDK
{
	public class HttpClientHelper : IHttpClientHelper
	{
		private HttpMessageHandler handler;
		public HttpMessageHandler MessageHandler
		{
			get { return handler ?? (handler = new CFNetworkHandler()); }
		}
	}
}


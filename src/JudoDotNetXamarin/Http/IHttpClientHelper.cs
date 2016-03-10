using System;
using System.Net.Http;

namespace JudoDotNetXamarin
{
	public interface IHttpClientHelper
	{
		HttpMessageHandler MessageHandler { get; }
	}
}


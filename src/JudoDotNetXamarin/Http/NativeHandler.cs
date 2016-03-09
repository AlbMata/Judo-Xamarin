using System;
using System.Net.Http;

namespace JudoDotNetXamarin
{
	public class NativeHandler : DelegatingHandler
	{
		public NativeHandler(HttpMessageHandler innerHandler) : base(innerHandler)
		{
		}
	}
}
	

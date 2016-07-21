using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo ("JudoDotNetXamariniOSSDK")]
[assembly: InternalsVisibleTo ("JudoDotNetXamarinAndroidSDK")]
[assembly: InternalsVisibleTo("JudoDotNetXamarin.Tests")]
namespace JudoDotNetXamarin
{
    internal class ServiceFactory
    {
        internal IPaymentService GetPaymentService ()
        {
            var judoApi = JudoPaymentsFactory.Create (JudoConfiguration.Instance.Environment, JudoConfiguration.Instance.ApiToken, JudoConfiguration.Instance.ApiSecret);
            return  new PaymentService (judoApi);
        }
    }
}


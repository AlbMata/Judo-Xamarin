using System.Net;
using JudoDotNetXamarin.Tests.Credentials;
using JudoPayDotNet.Enums;

namespace JudoDotNetXamarin.Tests
{
    public abstract class JudoTestBase
    {
        protected JudoTestBase()
        {
            var clientHelper = new HttpClientHelper();
            ServiceContainer.Register<IHttpClientHelper>(clientHelper);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
        }

        internal IPaymentService GetPaymentService()
        {
            var factory = new ServiceFactory();
            return factory.GetPaymentService();
        }

        internal JudoConfiguration GetConfiguration()
        {
            return JudoConfiguration.Instance;
        }

        internal void SetConfiguration(CredientialsSet credientialsSet)
        {
            var config = GetConfiguration();
            config.ApiToken = credientialsSet.Token;
            config.ApiSecret = credientialsSet.Secret;
            config.JudoId = credientialsSet.JudoId;
            config.Environment = JudoEnvironment.Sandbox;
        }

        internal IClientService GetClientService()
        {
            return new FakeClientService();
        }
    }
}
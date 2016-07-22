using JudoPayDotNet.Models;
using Newtonsoft.Json.Linq;

namespace JudoDotNetXamarin.Tests
{
    public class FakeClientService : IClientService
    {
        public JObject GetClientDetails()
        {
            return new JObject();
        }

        public string GetSDKVersion()
        {
            return "5.0";
        }

        public ConsumerLocationModel GetDeviceLocation()
        {
            return new ConsumerLocationModel();
        }
    }
}
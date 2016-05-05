using System;
using JudoDotNetXamarin;
using Newtonsoft.Json.Linq;
using JudoPayDotNet.Models;

namespace JudoDotNetXamarin
{
    public interface IClientService
    {
        JObject GetClientDetails ();

        string GetSDKVersion ();

        ConsumerLocationModel GetDeviceLocation ();
    }
}


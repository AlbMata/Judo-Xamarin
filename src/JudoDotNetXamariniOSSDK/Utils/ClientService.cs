using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using Foundation;
using JudoShieldiOS;
using PassKit;
using UIKit;
using JudoDotNetXamarin;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using JudoPayDotNet.Models;
using System.Drawing;


#if __UNIFIED__
// Mappings Unified CoreGraphic classes to MonoTouch classes
using CoreLocation;

#else
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.CoreFoundation;
using MonoTouch.CoreGraphics;
using MonoTouch.ObjCRuntime;
// Mappings Unified types to MonoTouch types
using nfloat = global::System.Single;
using nint = global::System.Int32;
using nuint = global::System.UInt32;
#endif

namespace JudoDotNetXamariniOSSDK
{
    public class ClientService : IClientService
    {
        public JObject GetClientDetails ()
        {
            if (!Judo.Instance.RiskSignals) {
                return null;
            }

            var clientDetails = new ClientDetails {
                OS = "iOS " + UIDevice.CurrentDevice.SystemVersion,
                KDeviceId = JudoShield.GetKDeviceIdentifier (),
                VDeviceId = JudoShield.GetVDeviceIdentifier (),
                DeviceModel = UIDevice.CurrentDevice.Model,
                CultureLocale = NSLocale.CurrentLocale.CountryCode,
                SslPinningEnabled = Judo.Instance.SSLPinningEnabled
            };

            return JObject.FromObject (clientDetails);
        }





        private string GetDeviceMacAddress ()
        {
            foreach (var netInterface in NetworkInterface.GetAllNetworkInterfaces ()) {
                if (netInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                    netInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet) {
                    var address = netInterface.GetPhysicalAddress ();
                    return BitConverter.ToString (address.GetAddressBytes ());

                }
            }

            return "NoMac";
        }

        public bool ApplePayAvailable {
            get {

                List<NSString> paymentNetworks = new List<NSString> ();
                paymentNetworks.Add (PassKit.PKPaymentNetwork.Visa);
                paymentNetworks.Add (PassKit.PKPaymentNetwork.MasterCard);

                if (Judo.Instance.AmExAccepted) {
                    paymentNetworks.Add (PassKit.PKPaymentNetwork.Amex);

                }

                if (PKPaymentAuthorizationViewController.CanMakePayments && PKPaymentAuthorizationViewController.CanMakePaymentsUsingNetworks (paymentNetworks.ToArray ())) {
                    return true;
                } else {

                    return false;
                }
            }
        }

        public string GetSDKVersion ()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly ();

            try {
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo (assembly.Location);
                string version = fvi.FileVersion;

                return "Xamarin-iOS-" + version;
            } catch (System.Exception e) {
                return "Xamarin-iOS-" + "UNKNOWN-AssembleyNotFound";
            }

        }

        public ConsumerLocationModel GetDeviceLocation ()
        {
            PointF location = JudoShield.GetDeviceLocation ();

            if (location != null) {
                return new ConsumerLocationModel {
                    Longitude = (decimal)location.X,
                    Latitude = (decimal)location.Y
                };
            }
            return new ConsumerLocationModel {
                Longitude = 0,
                Latitude = 0
            };

        }
    }
}
using System;
using System.Threading.Tasks;
using Foundation;
using JudoDotNetXamarin;
using JudoDotNetXamariniOSSDK.Controllers;
using JudoDotNetXamariniOSSDK.Delegates;
using JudoDotNetXamariniOSSDK.ViewModels;
using JudoPayDotNet;
using JudoPayDotNet.Models;
using Newtonsoft.Json.Linq;
using PassKit;
using UIKit;

namespace JudoDotNetXamariniOSSDK.Services
{
    internal class ApplePayService : IApplePayService
    {
        private JudoPayApi _judoAPI;
        private ClientService _clientService;
        private PKPaymentModel _sessionPKPaymentModel { get; set; }

        public ApplePayService (JudoPayApi judoAPI)
        {
            _judoAPI = judoAPI;
            _clientService = new ClientService ();
            _sessionPKPaymentModel = new PKPaymentModel ();
        }

        public void MakeApplePayment (ApplePayViewModel payment, JudoSuccessCallback success, JudoFailureCallback failure, UIViewController rootView, ApplePaymentType type)
        {
            try {
                PKPaymentRequest request = new PKPaymentRequest ();

                request.CurrencyCode = payment.CurrencyCode;

                request.CountryCode = payment.CountryCode;

                request.MerchantCapabilities = (PKMerchantCapability)payment.MerchantCapabilities;


                request.SupportedNetworks = payment.SupportedNetworks;


                request.PaymentSummaryItems = payment.Basket;

                request.MerchantIdentifier = payment.MerchantIdentifier;// @"merchant.com.judo.Xamarin"; // do it with configuration/overwrite

                var pkDelegate = new JudoPKPaymentAuthorizationViewControllerDelegate (this, request, payment.ConsumerRef.ToString (), type, success, failure);

                PKPaymentAuthorizationViewController pkController = new PKPaymentAuthorizationViewController (request) { Delegate = pkDelegate };
                rootView.PresentViewController (pkController, true, null);

            } catch (Exception e) {
                Console.WriteLine (e.InnerException.ToString ());

                var judoError = new JudoError () { Exception = e.InnerException };
                failure (judoError);
            }
        }



        public async Task<IResult<ITransactionResult>> HandlePKPayment (PKPayment payment, string customerRef, NSDecimalNumber amount, ApplePaymentType type, JudoFailureCallback failure)
        {
            try {

                var json = payment.Token.PaymentData.ToString (NSStringEncoding.UTF8);
                JObject jo = JObject.Parse (json.ToString ());

                _sessionPKPaymentModel.JudoId = JudoConfiguration.Instance.JudoId;
                _sessionPKPaymentModel.YourConsumerReference = customerRef;
                _sessionPKPaymentModel.Amount = amount.ToDecimal ();
                _sessionPKPaymentModel.ClientDetails = _clientService.GetClientDetails ();
                _sessionPKPaymentModel.ConsumerLocation = _clientService.GetDeviceLocation ();
                _sessionPKPaymentModel.UserAgent = _clientService.GetSDKVersion ();
                _sessionPKPaymentModel.PkPayment = new PKPaymentInnerModel () {
                    Token = new PKPaymentTokenModel () {
                        PaymentData = jo,
                        PaymentInstrumentName = payment.Token.PaymentInstrumentName,
                        PaymentNetwork = payment.Token.PaymentNetwork
                    }
                };


                Task<IResult<ITransactionResult>> task = null;
                if (type == ApplePaymentType.Payment) {

                    task = _judoAPI.Payments.Create (_sessionPKPaymentModel);
                } else if (type == ApplePaymentType.PreAuth) {
                    task = _judoAPI.PreAuths.Create (_sessionPKPaymentModel);
                }
                if (task == null) {
                    var judoError = new JudoError () { Exception = new Exception ("Judo server did not return response. Please contact customer support") };
                    failure (judoError);
                }
                return await task;
            } catch (Exception e) {
                Console.WriteLine (e.InnerException.ToString ());
                var judoError = new JudoError () { Exception = e };
                failure (judoError);
                return null;
            }
        }

        public void CycleSession ()
        {
            _sessionPKPaymentModel = new PKPaymentModel ();
        }
    }
}


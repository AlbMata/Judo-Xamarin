using JudoDotNetXamarin;
using Android.Content;
using Android.App;
using JudoDotNetXamarinAndroidSDK.Activities;
using System;
using Result = Android.App.Result;
using JudoPayDotNet.Models;
using Java.Interop;
using Java.Net;
using JudoPayDotNet.Errors;
using Java.Security;
using Newtonsoft.Json;

namespace JudoDotNetXamarinAndroidSDK
{
    [Activity]
    internal class UIMethods : Activity, JudoAndroidSDKAPI
    {
        private const int ACTION_CARD_PAYMENT = 101;
        private const int ACTION_TOKEN_PAYMENT = 102;
        private const int ACTION_PREAUTH = 201;
        private const int ACTION_TOKEN_PREAUTH = 202;
        private const int ACTION_REGISTER_CARD = 301;

        private static Lazy<JudoSuccessCallback> _judoSuccessCallback;

        private static Lazy<JudoFailureCallback> _judoFailureCallback;

        protected override void OnCreate (Android.OS.Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);
            if (savedInstanceState == null) {
                Intent i;
                var requestCode = Intent.GetStringExtra (Judo.REQUEST_CODE);
                PopulateIntent (out i);

                this.StartActivityForResult (i, Int32.Parse (requestCode));
            }

            try {
                ServiceContainer.Resolve<IHttpClientHelper> ();
            } catch {
                ServiceContainer.Register<IHttpClientHelper> (new HttpClientHelper ());
            }

        }

        public void Payment (PaymentViewModel payment, JudoSuccessCallback success, JudoFailureCallback failure, Activity context)
        {
            Intent i = new Intent (context, typeof (UIMethods));
            i.PutExtra (Judo.REQUEST_CODE, ACTION_CARD_PAYMENT.ToString ());
            i.PutExtra (Judo.JUDO_CONSUMER, JsonConvert.SerializeObject (new Consumer () { YourConsumerReference = payment.ConsumerReference }));
            i.PutExtra (Judo.JUDO_AMOUNT, payment.Amount.ToString ());
            i.PutExtra (Judo.JUDO_ID, (String.IsNullOrWhiteSpace (payment.JudoID) ? JudoConfiguration.Instance.JudoId : payment.JudoID));
            i.PutExtra (Judo.JUDO_CURRENCY, payment.Currency);
            _judoSuccessCallback = new Lazy<JudoSuccessCallback> (() => success);
            _judoFailureCallback = new Lazy<JudoFailureCallback> (() => failure);

            context.StartActivityForResult (i, ACTION_CARD_PAYMENT);

        }

        public void PreAuth (JudoDotNetXamarin.PaymentViewModel preAuthorisation, JudoDotNetXamarin.JudoSuccessCallback success, JudoDotNetXamarin.JudoFailureCallback failure, Activity context)
        {
            Intent i = new Intent (context, typeof (UIMethods));
            i.PutExtra (Judo.REQUEST_CODE, ACTION_PREAUTH.ToString ());
            i.PutExtra (Judo.JUDO_CONSUMER, JsonConvert.SerializeObject (new Consumer () { YourConsumerReference = preAuthorisation.ConsumerReference }));
            i.PutExtra (Judo.JUDO_AMOUNT, preAuthorisation.Amount.ToString ());
            i.PutExtra (Judo.JUDO_ID, (String.IsNullOrWhiteSpace (preAuthorisation.JudoID) ? JudoConfiguration.Instance.JudoId : preAuthorisation.JudoID));
            i.PutExtra (Judo.JUDO_CURRENCY, preAuthorisation.Currency);
            _judoSuccessCallback = new Lazy<JudoSuccessCallback> (() => success);
            _judoFailureCallback = new Lazy<JudoFailureCallback> (() => failure);
            context.StartActivityForResult (i, ACTION_PREAUTH);
        }


        public void TokenPayment (JudoDotNetXamarin.TokenPaymentViewModel payment, JudoDotNetXamarin.JudoSuccessCallback success, JudoDotNetXamarin.JudoFailureCallback failure, Activity context)
        {
            Intent i = new Intent (context, typeof (UIMethods));
            i.PutExtra (Judo.REQUEST_CODE, ACTION_TOKEN_PAYMENT.ToString ());
            i.PutExtra (Judo.JUDO_CONSUMER, JsonConvert.SerializeObject (new Consumer () {
                YourConsumerReference = payment.ConsumerReference,
                ConsumerToken = payment.ConsumerToken
            }));
            i.PutExtra (Judo.JUDO_AMOUNT, payment.Amount.ToString ());
            i.PutExtra (Judo.JUDO_ID, (String.IsNullOrWhiteSpace (payment.JudoID) ? JudoConfiguration.Instance.JudoId : payment.JudoID));
            i.PutExtra (Judo.JUDO_CURRENCY, payment.Currency);
            i.PutExtra (Judo.JUDO_CARD_DETAILS, JsonConvert.SerializeObject (new CardToken () {
                CardLastFour = payment.LastFour,
                CardType = payment.CardType,
                Token = payment.Token,
                ConsumerToken = payment.ConsumerToken
            }));
            _judoSuccessCallback = new Lazy<JudoSuccessCallback> (() => success);
            _judoFailureCallback = new Lazy<JudoFailureCallback> (() => failure);

            context.StartActivityForResult (i, ACTION_TOKEN_PAYMENT);
        }


        public void TokenPreAuth (JudoDotNetXamarin.TokenPaymentViewModel payment, JudoDotNetXamarin.JudoSuccessCallback success, JudoDotNetXamarin.JudoFailureCallback failure, Activity context)
        {
            Intent i = new Intent (context, typeof (UIMethods));
            i.PutExtra (Judo.REQUEST_CODE, ACTION_TOKEN_PREAUTH.ToString ());
            i.PutExtra (Judo.JUDO_CONSUMER, JsonConvert.SerializeObject (new Consumer () {
                YourConsumerReference = payment.ConsumerReference,
                ConsumerToken = payment.ConsumerToken
            }));
            i.PutExtra (Judo.JUDO_AMOUNT, payment.Amount.ToString ());
            i.PutExtra (Judo.JUDO_ID, (String.IsNullOrWhiteSpace (payment.JudoID) ? JudoConfiguration.Instance.JudoId : payment.JudoID));
            i.PutExtra (Judo.JUDO_CURRENCY, payment.Currency);
            i.PutExtra (Judo.JUDO_CARD_DETAILS, payment.Token);
            i.PutExtra (Judo.JUDO_CARD_DETAILS, JsonConvert.SerializeObject (new CardToken () {
                CardLastFour = payment.LastFour,
                CardType = payment.CardType,
                Token = payment.Token,
                ConsumerToken = payment.ConsumerToken
            }));
            _judoSuccessCallback = new Lazy<JudoSuccessCallback> (() => success);
            _judoFailureCallback = new Lazy<JudoFailureCallback> (() => failure);

            context.StartActivityForResult (i, ACTION_TOKEN_PREAUTH);
        }


        public void RegisterCard (JudoDotNetXamarin.PaymentViewModel payment, JudoDotNetXamarin.JudoSuccessCallback success, JudoDotNetXamarin.JudoFailureCallback failure, Activity context)
        {
            Intent i = new Intent (context, typeof (UIMethods));
            i.PutExtra (Judo.REQUEST_CODE, ACTION_REGISTER_CARD.ToString ());
            i.PutExtra (Judo.JUDO_CONSUMER, JsonConvert.SerializeObject (new Consumer () { YourConsumerReference = payment.ConsumerReference }));
            i.PutExtra (Judo.JUDO_AMOUNT, payment.Amount.ToString ());
            i.PutExtra (Judo.JUDO_ID, (String.IsNullOrWhiteSpace (payment.JudoID) ? JudoConfiguration.Instance.JudoId : payment.JudoID));
            i.PutExtra (Judo.JUDO_CURRENCY, payment.Currency);


            _judoSuccessCallback = new Lazy<JudoSuccessCallback> (() => success);
            _judoFailureCallback = new Lazy<JudoFailureCallback> (() => failure);

            context.StartActivityForResult (i, ACTION_REGISTER_CARD);
        }

        void PopulateIntent (out Intent i)
        {

            var requestCode = Intent.GetStringExtra (Judo.REQUEST_CODE);
            var judoCurrency = Intent.GetStringExtra (Judo.JUDO_CURRENCY);
            var judoConsumer = Intent.GetStringExtra (Judo.JUDO_CONSUMER);
            string amount = Intent.GetStringExtra (Judo.JUDO_AMOUNT);

            var judoAmount = (amount != null ? decimal.Parse (amount) : 0);
            var judoId = Intent.GetStringExtra (Judo.JUDO_ID);

            i = GetIntentForRequestCode (requestCode);
            i.PutExtra (Judo.JUDO_CONSUMER, judoConsumer);
            i.PutExtra (Judo.JUDO_AMOUNT, judoAmount.ToString ());
            i.PutExtra (Judo.JUDO_ID, judoId);
            i.PutExtra (Judo.JUDO_CURRENCY, judoCurrency);
            var intCode = Int32.Parse (requestCode);
            if (intCode == ACTION_TOKEN_PAYMENT || intCode == ACTION_TOKEN_PREAUTH) {
                i.PutExtra (Judo.JUDO_CARD_DETAILS, Intent.GetStringExtra (Judo.JUDO_CARD_DETAILS));
            }
        }

        Intent GetIntentForRequestCode (string requestCode)
        {
            Intent i = new Intent ();
            switch (Int32.Parse (requestCode)) {
            case ACTION_CARD_PAYMENT:
                i = new Intent (this, typeof (PaymentActivity));
                break;
            case ACTION_PREAUTH:
                i = new Intent (this, typeof (PreAuthActivity));
                break;
            case ACTION_TOKEN_PAYMENT:
                i = new Intent (this, typeof (PaymentTokenActivity));
                break;
            case ACTION_TOKEN_PREAUTH:
                i = new Intent (this, typeof (PreAuthTokenActivity));
                break;
            case ACTION_REGISTER_CARD:
                i = new Intent (this, typeof (RegisterCardActivity));
                break;

            }
            return i;
        }

        protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
        {
            JudoReceipt receipt = null;
            string msg_prefix = "";

            if (data != null) {
                var resultString = data.GetStringExtra (Judo.JUDO_RECEIPT);
                if (resultString != null) {
                    var jsonResult = JsonConvert.DeserializeObject<PaymentReceiptModel> (resultString);
                    receipt = new JudoReceipt (jsonResult);
                }
                if (resultCode == Result.Canceled) {

                    Finish ();
                } else if (resultCode == Judo.JUDO_ERROR) {
                    var jsonError = data.GetStringExtra (Judo.JUDO_ERROR_EXCEPTION);
                    var error = (jsonError != null ? JsonConvert.DeserializeObject<JudoError> (data.GetStringExtra (Judo.JUDO_ERROR_EXCEPTION)) : null);
                    PaymentReceiptModel paymentReceipt = null;
                    if (receipt != null) {
                        paymentReceipt = receipt.FullReceipt as PaymentReceiptModel;
                    }

                    var innerError = new JudoError () {
                        Exception = new Exception ("Unknown Error"),
                        ApiError = null
                    };
                    if (error != null) {

                        innerError = new JudoError () {
                            Exception = error.Exception,
                            ApiError = error.ApiError
                        };
                    }
                    _judoFailureCallback.Value (innerError, paymentReceipt);

                    Finish ();
                } else {
                    HandleRequestCode (requestCode, resultCode, receipt);
                }
            }
            Finish ();
        }

        void HandleRequestCode (int requestCode, Result resultCode, JudoReceipt receipt)
        {
            if (resultCode == Result.Ok && receipt.Result != "Declined") {
                PaymentReceiptModel paymentReceipt;
                if ((paymentReceipt = receipt.FullReceipt as PaymentReceiptModel) != null) {

                    _judoSuccessCallback.Value (paymentReceipt);
                    Finish ();

                }
            } else if (_judoFailureCallback.Value != null) {
                var judoError = new JudoError ();
                var paymentreceipt = receipt != null ? receipt.FullReceipt as PaymentReceiptModel : null;

                if (paymentreceipt != null) {

                    _judoFailureCallback.Value (judoError, paymentreceipt);
                    Finish ();
                } else {

                    _judoFailureCallback.Value (judoError);
                    Finish ();
                }
            }

        }
        /// <summary>
        /// intentionally empty, no point, matches interface
        /// </summary>
        /// <returns>The session.</returns>
        public void CycleSession ()
        {
        }
    }
}


using System;
using System.Threading.Tasks;
using JudoDotNetXamarin;
using JudoPayDotNet;
using JudoPayDotNet.Models;
using JudoDotNetXamarin;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo ("JudoDotNetXamariniOSSDK")]
[assembly: InternalsVisibleTo ("JudoDotNetXamarinAndroidSDK")]
[assembly: InternalsVisibleTo("JudoDotNetXamarin.Tests")]
namespace JudoDotNetXamarin
{
    internal class PaymentService : IPaymentService
    {
        private	JudoPayApi _judoAPI;

        public PaymentService (JudoPayApi judoAPI)
        {
            _judoAPI = judoAPI;
        }

        public async Task<IResult<ITransactionResult>> MakePayment (PaymentViewModel paymentViewModel, IClientService clientService)
        {
            JudoConfiguration.Instance.Validate ();
            CardPaymentModel payment = new CardPaymentModel {
                JudoId = (String.IsNullOrWhiteSpace (paymentViewModel.JudoID) ? JudoConfiguration.Instance.JudoId : paymentViewModel.JudoID),
                YourConsumerReference = (String.IsNullOrWhiteSpace (paymentViewModel.ConsumerReference) ? ("Consumer:" + JudoConfiguration.Instance.JudoId) : paymentViewModel.ConsumerReference),
                Amount = paymentViewModel.Amount,
                CardNumber = paymentViewModel.Card.CardNumber,
                CV2 = paymentViewModel.Card.CV2,
                ExpiryDate = paymentViewModel.Card.ExpireDate,
                CardAddress = new CardAddressModel () {
                    PostCode = paymentViewModel.Card.PostCode,
                    CountryCode = (int)paymentViewModel.Card.CountryCode
                },
                StartDate = paymentViewModel.Card.StartDate,
                IssueNumber = paymentViewModel.Card.IssueNumber,
                YourPaymentMetaData = paymentViewModel.YourPaymentMetaData,
                ClientDetails = clientService.GetClientDetails (),
                Currency = paymentViewModel.Currency,
                UserAgent = clientService.GetSDKVersion ()
            };

            Task<IResult<ITransactionResult>> task = _judoAPI.Payments.Create (payment);

            return await task;

        }

        public async Task<IResult<ITransactionResult>> PreAuthoriseCard (PaymentViewModel authorisation, IClientService clientService)
        {      
            JudoConfiguration.Instance.Validate ();
            CardPaymentModel payment = new CardPaymentModel {
                JudoId = (String.IsNullOrWhiteSpace (authorisation.JudoID) ? JudoConfiguration.Instance.JudoId : authorisation.JudoID),
                YourConsumerReference = (String.IsNullOrWhiteSpace (authorisation.ConsumerReference) ? ("Consumer:" + JudoConfiguration.Instance.JudoId) : authorisation.ConsumerReference),
                Amount = authorisation.Amount,
                CardNumber = authorisation.Card.CardNumber,
                CV2 = authorisation.Card.CV2,
                ExpiryDate = authorisation.Card.ExpireDate,
                CardAddress = new CardAddressModel () {
                    PostCode = authorisation.Card.PostCode,
                    CountryCode = (int)authorisation.Card.CountryCode
                },
                StartDate = authorisation.Card.StartDate,
                IssueNumber = authorisation.Card.IssueNumber,
                YourPaymentMetaData = authorisation.YourPaymentMetaData,
                ClientDetails = clientService.GetClientDetails (),
                UserAgent = clientService.GetSDKVersion (),
                Currency = authorisation.Currency
            };

            Task<IResult<ITransactionResult>> task = _judoAPI.PreAuths.Create (payment);
            return await task;
        }

        public async Task<IResult<ITransactionResult>> MakeTokenPayment (TokenPaymentViewModel tokenPayment, IClientService clientService)
        {
            JudoConfiguration.Instance.Validate ();
            TokenPaymentModel payment = new TokenPaymentModel {
                JudoId = (String.IsNullOrWhiteSpace (tokenPayment.JudoID) ? JudoConfiguration.Instance.JudoId : tokenPayment.JudoID),
                YourConsumerReference = (String.IsNullOrWhiteSpace (tokenPayment.ConsumerReference) ? ("Consumer:" + JudoConfiguration.Instance.JudoId) : tokenPayment.ConsumerReference),
                Amount = tokenPayment.Amount,
                CardToken = tokenPayment.Token,
                CV2 = tokenPayment.CV2,
                ConsumerToken = tokenPayment.ConsumerToken,
                YourPaymentMetaData = tokenPayment.YourPaymentMetaData,
                ClientDetails = clientService.GetClientDetails (),
                UserAgent = clientService.GetSDKVersion ()
            };
            Task<IResult<ITransactionResult>> task = _judoAPI.Payments.Create (payment);
            return await task;
    
        }

        public async Task<IResult<ITransactionResult>> MakeTokenPreAuthorisation (TokenPaymentViewModel tokenPayment, IClientService clientService)
        {
          
            JudoConfiguration.Instance.Validate ();
            TokenPaymentModel payment = new TokenPaymentModel {
                JudoId = (String.IsNullOrWhiteSpace (tokenPayment.JudoID) ? JudoConfiguration.Instance.JudoId : tokenPayment.JudoID),
                YourConsumerReference = (String.IsNullOrWhiteSpace (tokenPayment.ConsumerReference) ? ("Consumer:" + JudoConfiguration.Instance.JudoId) : tokenPayment.ConsumerReference),
                Amount = tokenPayment.Amount,
                CardToken = tokenPayment.Token,
                CV2 = tokenPayment.CV2,
                ConsumerToken = tokenPayment.ConsumerToken,
                YourPaymentMetaData = tokenPayment.YourPaymentMetaData,
                ClientDetails = clientService.GetClientDetails (),
                UserAgent = clientService.GetSDKVersion ()
            };
           
            Task<IResult<ITransactionResult>> task = _judoAPI.PreAuths.Create (payment);
            return await task;
        }

        public async Task<IResult<ITransactionResult>> RegisterCard (PaymentViewModel payment, IClientService clientService)
        {

          
            JudoConfiguration.Instance.Validate ();
            var registerCard = new CardPaymentModel () {                 
                JudoId = (String.IsNullOrWhiteSpace (payment.JudoID) ? JudoConfiguration.Instance.JudoId : payment.JudoID),
                YourConsumerReference = (String.IsNullOrWhiteSpace (payment.ConsumerReference) ? ("Consumer:" + JudoConfiguration.Instance.JudoId) : payment.ConsumerReference),
                Amount = payment.Amount,
                CardNumber = payment.Card.CardNumber,
                CV2 = payment.Card.CV2,
                ExpiryDate = payment.Card.ExpireDate,
                CardAddress = new CardAddressModel () {
                    PostCode = payment.Card.PostCode,
                    CountryCode = (int)payment.Card.CountryCode
                },
                StartDate = payment.Card.StartDate,
                IssueNumber = payment.Card.IssueNumber,
                YourPaymentMetaData = payment.YourPaymentMetaData,
                ClientDetails = clientService.GetClientDetails (),
                UserAgent = clientService.GetSDKVersion (),
                Currency = payment.Currency
            };

            Task<IResult<ITransactionResult>> task = _judoAPI.RegisterCards.Create (registerCard);
            return await task;
          
        }

        public async Task<IResult<ITransactionResult>> CompleteDSecure (long receiptID, string paRes, string md)
        {
            try {
                ThreeDResultModel model = new ThreeDResultModel ();
                model.PaRes = paRes;

                Task<IResult<PaymentReceiptModel>> task = _judoAPI.ThreeDs.Complete3DSecure (receiptID, model);
                return await task;
            } catch (Exception e) {
                var error = new JudoError () { 
                    Exception = e,
                    ApiError = new JudoPayDotNet.Errors.ModelError () {
                        Message = e.InnerException.ToString ()
                    }
                };
                throw error;
            }
        }
       

    }
}

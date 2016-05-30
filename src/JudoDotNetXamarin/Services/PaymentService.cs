using System;
using System.Threading.Tasks;
using JudoDotNetXamarin;
using JudoPayDotNet;
using JudoPayDotNet.Models;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo ("JudoDotNetXamariniOSSDK")]
[assembly: InternalsVisibleTo ("JudoDotNetXamarinAndroidSDK")]
namespace JudoDotNetXamarin
{
    internal class PaymentService : IPaymentService
    {
        private JudoPayApi _judoAPI;

        private CardPaymentModel _sessionPaymentModel { get; set; }
        private TokenPaymentModel _sessionTokenPaymentModel { get; set; }
        public PaymentService (JudoPayApi judoAPI)
        {
            _judoAPI = judoAPI;
            CycleSession ();
        }

        public void CycleSession ()
        {
            _sessionPaymentModel = new CardPaymentModel ();
            _sessionTokenPaymentModel = new TokenPaymentModel ();
        }

        public async Task<IResult<ITransactionResult>> MakePayment (PaymentViewModel paymentViewModel, IClientService clientService)
        {


            PopulatePaymentModel (paymentViewModel, clientService);
            Task<IResult<ITransactionResult>> task = _judoAPI.Payments.Create (_sessionPaymentModel);
            return await task;

        }

        private void PopulatePaymentModel (PaymentViewModel paymentViewModel, IClientService clientService)
        {
            JudoConfiguration.Instance.Validate ();
            _sessionPaymentModel.JudoId = (String.IsNullOrWhiteSpace (paymentViewModel.JudoID) ? JudoConfiguration.Instance.JudoId : paymentViewModel.JudoID);
            _sessionPaymentModel.YourConsumerReference = (String.IsNullOrWhiteSpace (paymentViewModel.ConsumerReference) ? ("Consumer:" + JudoConfiguration.Instance.JudoId) : paymentViewModel.ConsumerReference);
            _sessionPaymentModel.Amount = paymentViewModel.Amount;
            _sessionPaymentModel.CardNumber = paymentViewModel.Card.CardNumber;
            _sessionPaymentModel.CV2 = paymentViewModel.Card.CV2;
            _sessionPaymentModel.ExpiryDate = paymentViewModel.Card.ExpireDate;
            _sessionPaymentModel.CardAddress = new CardAddressModel () {
                PostCode = paymentViewModel.Card.PostCode,
                CountryCode = (int)paymentViewModel.Card.CountryCode
            };
            _sessionPaymentModel.StartDate = paymentViewModel.Card.StartDate;
            _sessionPaymentModel.IssueNumber = paymentViewModel.Card.IssueNumber;
            _sessionPaymentModel.YourPaymentMetaData = paymentViewModel.YourPaymentMetaData;
            _sessionPaymentModel.ClientDetails = clientService.GetClientDetails ();
            _sessionPaymentModel.ConsumerLocation = clientService.GetDeviceLocation ();
            _sessionPaymentModel.Currency = paymentViewModel.Currency;
            _sessionPaymentModel.UserAgent = clientService.GetSDKVersion ();
        }

        private void PopulateTokenPaymentModel (TokenPaymentViewModel tokenPayment, IClientService clientService)
        {
            JudoConfiguration.Instance.Validate ();

            _sessionTokenPaymentModel.JudoId = (String.IsNullOrWhiteSpace (tokenPayment.JudoID) ? JudoConfiguration.Instance.JudoId : tokenPayment.JudoID);
            _sessionTokenPaymentModel.YourConsumerReference = (String.IsNullOrWhiteSpace (tokenPayment.ConsumerReference) ? ("Consumer:" + JudoConfiguration.Instance.JudoId) : tokenPayment.ConsumerReference);
            _sessionTokenPaymentModel.Amount = tokenPayment.Amount;
            _sessionTokenPaymentModel.CardToken = tokenPayment.Token;
            _sessionTokenPaymentModel.CV2 = tokenPayment.CV2;
            _sessionTokenPaymentModel.ConsumerToken = tokenPayment.ConsumerToken;
            _sessionTokenPaymentModel.YourPaymentMetaData = tokenPayment.YourPaymentMetaData;
            _sessionTokenPaymentModel.ClientDetails = clientService.GetClientDetails ();
            _sessionTokenPaymentModel.ConsumerLocation = clientService.GetDeviceLocation ();
            _sessionTokenPaymentModel.UserAgent = clientService.GetSDKVersion ();
        }


        public async Task<IResult<ITransactionResult>> PreAuthoriseCard (PaymentViewModel authorisation, IClientService clientService)
        {
            PopulatePaymentModel (authorisation, clientService);

            Task<IResult<ITransactionResult>> task = _judoAPI.PreAuths.Create (_sessionPaymentModel);
            return await task;
        }

        public async Task<IResult<ITransactionResult>> MakeTokenPayment (TokenPaymentViewModel tokenPayment, IClientService clientService)
        {

            PopulateTokenPaymentModel (tokenPayment, clientService);
            Task<IResult<ITransactionResult>> task = _judoAPI.Payments.Create (_sessionTokenPaymentModel);
            return await task;

        }

        public async Task<IResult<ITransactionResult>> MakeTokenPreAuthorisation (TokenPaymentViewModel tokenPayment, IClientService clientService)
        {
            PopulateTokenPaymentModel (tokenPayment, clientService);

            Task<IResult<ITransactionResult>> task = _judoAPI.PreAuths.Create (_sessionTokenPaymentModel);
            return await task;
        }

        public async Task<IResult<ITransactionResult>> RegisterCard (PaymentViewModel payment, IClientService clientService)
        {
            PopulatePaymentModel (payment, clientService);
            Task<IResult<ITransactionResult>> task = _judoAPI.RegisterCards.Create (_sessionPaymentModel);
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

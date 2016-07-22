using System.Collections.Generic;

namespace JudoDotNetXamarin.Tests.Builders
{
    public class PaymentViewModelBuilder
    {
        private PaymentViewModel _paymentViewModel = new PaymentViewModel();

        public PaymentViewModelBuilder WithCardViewModel(CardViewModel cardViewModel)
        {
            _paymentViewModel.Card = cardViewModel;
            return this;
        }

        public PaymentViewModelBuilder WithAmount(decimal amount)
        {
            _paymentViewModel.Amount = amount;
            return this;
        }

        public PaymentViewModelBuilder WithCurrency(string currency)
        {
            _paymentViewModel.Currency = currency;
            return this;
        }

        public PaymentViewModelBuilder WithConsumerReference(string consumerReference)
        {
            _paymentViewModel.ConsumerReference = consumerReference;
            return this;
        }

        public PaymentViewModelBuilder WithPaymentMetaData(IDictionary<string, string> yourPaymentMetaData)
        {
            _paymentViewModel.YourPaymentMetaData = yourPaymentMetaData;
            return this;
        }

        public PaymentViewModel Build()
        {
            return _paymentViewModel;
        }
    }
}

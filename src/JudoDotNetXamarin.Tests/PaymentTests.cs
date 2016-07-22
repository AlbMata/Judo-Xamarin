using System;
using System.Threading.Tasks;
using JudoDotNetXamarin.Tests.Builders;
using JudoDotNetXamarin.Tests.Credentials;
using JudoDotNetXamarin.Tests.Payments;
using NUnit.Framework;

namespace JudoDotNetXamarin.Tests
{
    [TestFixture]
    public class PaymentTests : JudoTestBase
    {
        [Test]
        public async Task ValidPayment()
        {
            //Given I have a valid set of credentials not set up for 3d secure.
            var credentials = CredientialsManager.GetCredientialsSetFromKey(CreditialsSetKey.ValidSetNoThreeDSecure);
            SetConfiguration(credentials);

            var sut = GetPaymentService();

            //When I set up a card payment.
            var cardViewModel = PaymentManager.GetCardViewModelFromKey(PaymentKey.ValidVisaCardPayment);
            var paymentViewModel =
                new PaymentViewModelBuilder()
                    .WithCardViewModel(cardViewModel)
                    .WithAmount(1.01m)
                    .WithConsumerReference(Guid.NewGuid().ToString())
                    .WithCurrency("GBP")
                    .Build();

            //And call the payment service.
            var result = await sut.MakePayment(paymentViewModel, GetClientService());

            //Then the payment must succeed.
            Assertions.AssertSuccessfulCardPayment(result);
        }

        [Test]
        public async Task DeclinedPayment()
        {
            //Given I have a valid set of credentials not set up for 3d secure.
            var credentials = CredientialsManager.GetCredientialsSetFromKey(CreditialsSetKey.ValidSetNoThreeDSecure);
            SetConfiguration(credentials);

            var sut = GetPaymentService();

            //When I set up a card payment.
            var cardViewModel = PaymentManager.GetCardViewModelFromKey(PaymentKey.InvalidVisaCardPayment);
            var paymentViewModel =
                new PaymentViewModelBuilder()
                    .WithCardViewModel(cardViewModel)
                    .WithAmount(1.01m)
                    .WithConsumerReference(Guid.NewGuid().ToString())
                    .WithCurrency("GBP")
                    .Build();

            //And call the payment service.
            var result = await sut.MakePayment(paymentViewModel, GetClientService());

            //Then the payment must be declined.
            Assertions.AssertDeclinedCardPayment(result);
        }

        [Test]
        public async Task PaymentWithoutCurrency()
        {
            //Given I have a valid set of credentials not set up for 3d secure.
            var credentials = CredientialsManager.GetCredientialsSetFromKey(CreditialsSetKey.ValidSetNoThreeDSecure);
            SetConfiguration(credentials);

            var sut = GetPaymentService();

            //When I set up a card payment without a currency.
            var cardViewModel = PaymentManager.GetCardViewModelFromKey(PaymentKey.InvalidVisaCardPayment);
            var paymentViewModel =
                new PaymentViewModelBuilder()
                    .WithCardViewModel(cardViewModel)
                    .WithAmount(1.01m)
                    .WithConsumerReference(Guid.NewGuid().ToString())
                    .Build();

            //And call the payment service.
            var result = await sut.MakePayment(paymentViewModel, GetClientService());

            //Then the payment must error.
            Assertions.AssertErrorResponseWithModelErrors(result, 1, 1);
        }

        [Test]
        public async Task PaymentWithoutReference()
        {
            //Given I have a valid set of credentials not set up for 3d secure.
            var credentials = CredientialsManager.GetCredientialsSetFromKey(CreditialsSetKey.ValidSetNoThreeDSecure);
            SetConfiguration(credentials);

            var sut = GetPaymentService();

            //When I set up a card payment without a consumer reference.
            var cardViewModel = PaymentManager.GetCardViewModelFromKey(PaymentKey.InvalidVisaCardPayment);
            var paymentViewModel =
                new PaymentViewModelBuilder()
                    .WithCardViewModel(cardViewModel)
                    .WithAmount(1.01m)
                    .WithConsumerReference(string.Empty)
                    .WithCurrency("GBP")
                    .Build();

            //And call the payment service.
            var result = await sut.MakePayment(paymentViewModel, GetClientService());

            //Then the payment must error.
            Assertions.AssertDeclinedCardPayment(result);
        }
    }
}
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

            //Then the payments must succeed.
            Assert.IsNotNull(result);
            Assert.IsFalse(result.HasError);
            Assert.IsNotNull(result.Response);
            Assert.IsTrue(result.Response.ReceiptId > 0);
        }
    }
}
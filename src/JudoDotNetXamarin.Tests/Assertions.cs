using NUnit.Framework;

namespace JudoDotNetXamarin.Tests
{
    public static class Assertions
    {
        public static void AssertSuccessfulCardPayment(JudoPayDotNet.Models.IResult<JudoPayDotNet.Models.ITransactionResult> result)
        {
            Assert.IsNotNull(result);
            Assert.IsFalse(result.HasError);
            Assert.IsNotNull(result.Response);
            Assert.IsNotNull(result.Response.Result == "Success");
            Assert.IsTrue(result.Response.ReceiptId > 0);
        }

        public static void AssertDeclinedCardPayment(JudoPayDotNet.Models.IResult<JudoPayDotNet.Models.ITransactionResult> result)
        {
            Assert.IsNotNull(result);
            Assert.IsFalse(result.HasError);
            Assert.IsNotNull(result.Response);
            Assert.IsNotNull(result.Response.Result == "Declined");
            Assert.IsTrue(result.Response.ReceiptId > 0);
        }
    }
}

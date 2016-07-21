namespace JudoDotNetXamarin.Tests
{
    public abstract class JudoTestBase
    {
        internal IPaymentService GetPaymentService()
        {
            var factory = new ServiceFactory();
            return factory.GetPaymentService();
        }

        internal JudoConfiguration GetConfiguration()
        {
            return JudoConfiguration.Instance;
        }
    }
}
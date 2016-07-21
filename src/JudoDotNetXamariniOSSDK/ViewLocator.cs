using JudoDotNetXamariniOSSDK.Services;
using JudoDotNetXamariniOSSDK.Views;
using JudoDotNetXamarin;

namespace JudoDotNetXamariniOSSDK
{
    internal class ViewLocator
    {
        readonly IPaymentService _paymentService;

        public ViewLocator (IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public CreditCardView GetPaymentView ()
        {
            _paymentService.CycleSession ();
            CreditCardView ctrl = new CreditCardView (_paymentService);
            return ctrl;
        }

        public PreAuthorisationView GetPreAuthView ()
        {
            _paymentService.CycleSession ();
            PreAuthorisationView ctrl = new PreAuthorisationView (_paymentService);
            return ctrl;
        }

        public RegisterCardView GetRegisterCardView ()
        {
            _paymentService.CycleSession ();
            RegisterCardView ctrl = new RegisterCardView (_paymentService);
            return ctrl;
        }

        public TokenPaymentView GetTokenPaymentView ()
        {
            _paymentService.CycleSession ();
            TokenPaymentView ctrl = new TokenPaymentView (_paymentService);
            return ctrl;
        }

        public TokenPreAuthorisationView GetTokenPreAuthView ()
        {
            _paymentService.CycleSession ();
            TokenPreAuthorisationView ctrl = new TokenPreAuthorisationView (_paymentService);
            return ctrl;
        }


        public void CycleSession ()
        {
            _paymentService.CycleSession ();
        }
    }
}
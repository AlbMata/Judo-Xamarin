using System.Collections.Generic;
using System.Linq;
using JudoDotNetXamarin.Tests.Credentials;
using JudoPayDotNet.Models;

namespace JudoDotNetXamarin.Tests.Payments
{
    public static class PaymentManager
    {
        private static readonly Dictionary<PaymentKey, CardViewModel> CardViewModels = new Dictionary<PaymentKey, CardViewModel>();
        static PaymentManager()
        {
            CardViewModels.Add(PaymentKey.ValidVisaCardPayment, NewCardNewModel("4976000000003436", "12/20", "452", CardType.VISA));
            CardViewModels.Add(PaymentKey.InvalidVisaCardPayment, NewCardNewModel("4221690000004963", "12/20", "125", CardType.VISA));
        }

        public static CardViewModel GetCardViewModelFromKey(PaymentKey key)
        {
            return CardViewModels.Single(x => x.Key == key).Value;
        }

        private static CardViewModel NewCardNewModel(string cardNumber, string expiryDate, string cv2, CardType cardType)
        {
            return new CardViewModel
            {
                CardNumber = cardNumber,
                CV2 = cv2,
                CardType = cardType,
                ExpireDate = expiryDate
            };
        }
    }
}
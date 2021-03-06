using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Widget;
using JudoPayDotNet.Models;
using JudoDotNetXamarin;

namespace JudoDotNetXamarinAndroidSDK.Activities
{
    [Activity (Label = "PreAuthTokenActivity")]
    public class PreAuthTokenActivity : PaymentTokenActivity
    {

        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);
            SetTitle (Resource.String.title_pre_auth_token);
            FindViewById<Button> (Resource.Id.payButton).Text = Resources.GetString (Resource.String.token_preauth);

        }

        public override void MakeTokenPayment ()
        {
            TokenPaymentViewModel payment = new TokenPaymentViewModel () {
                Currency = judoCurrency,
                Amount = judoAmount,
                ConsumerToken = judoConsumer.ConsumerToken,
                CardType = judoCardToken.CardType,
                JudoID = judoId,
                Token = judoCardToken.Token,
                ConsumerReference = judoConsumer.YourConsumerReference,
                CV2 = cv2EntryView.GetCV2 ()

            };

            ShowLoadingSpinner (true);

            _paymentService.MakeTokenPreAuthorisation (payment, new ClientService ()).ContinueWith (HandleServerResponse, TaskScheduler.FromCurrentSynchronizationContext ());
        }
    }
}
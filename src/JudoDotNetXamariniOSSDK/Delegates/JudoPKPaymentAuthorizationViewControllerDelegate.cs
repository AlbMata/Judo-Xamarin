﻿using System;
using JudoPayDotNet.Logging;
using PassKit;
using Foundation;
using System.Threading.Tasks;
using JudoPayDotNet.Models;

namespace JudoDotNetXamariniOSSDK
{
	public partial class JudoPKPaymentAuthorizationViewControllerDelegate :NSObject, IPKPaymentAuthorizationViewControllerDelegate
	{
		IApplePayService _applePayService;
		NSDecimalNumber _runningTotal;
		ApplePaymentType _paymentAction;
		SuccessCallback _successCallBack;
		FailureCallback _failureCallback;

		public JudoPKPaymentAuthorizationViewControllerDelegate (IApplePayService applePayService, PKPaymentRequest request, ApplePaymentType type, SuccessCallback success, FailureCallback failure)
		{
			_applePayService = applePayService;
			_runningTotal = request.PaymentSummaryItems [request.PaymentSummaryItems.Length - 1].Amount;
			_paymentAction = type;
			_successCallBack = success;
			_failureCallback = failure;
		}



		public void DidAuthorizePayment (PKPaymentAuthorizationViewController controller, PKPayment payment, Action<PKPaymentAuthorizationStatus> completion)
		{

			ClearPaymentWithJudo (payment, completion);
		}


		public void PaymentAuthorizationViewControllerDidFinish (PKPaymentAuthorizationViewController controller)
		{
			controller.DismissViewController (true, null);
		}

		public void WillAuthorizePayment (PKPaymentAuthorizationViewController controller)
		{
			
		}

		async Task ClearPaymentWithJudo (PKPayment payment, Action<PKPaymentAuthorizationStatus> completion)
		{
			IResult res;
			var result = await _applePayService.HandlePKPayment (payment, _runningTotal, _paymentAction);
			if (result != null && !result.HasError && result.Response.Result != "Declined") {
				
				var paymentreceipt = result.Response as PaymentReceiptModel;

				if (paymentreceipt != null) {
					if (_successCallBack != null) {
						
						completion (PKPaymentAuthorizationStatus.Success);
						_successCallBack (paymentreceipt);
					}
				}
			} else {

				if (_failureCallback != null) {
					var judoError = new JudoError { ApiError = result != null ? result.Error : null };
					var paymentreceipt = result != null ? result.Response as PaymentReceiptModel : null;

					if (paymentreceipt != null) {
						// send receipt even we got card declined
						completion (PKPaymentAuthorizationStatus.Failure);
						_failureCallback (judoError, paymentreceipt);
					} else {
						completion (PKPaymentAuthorizationStatus.Failure);
						_failureCallback (judoError);
					}
				}
			}



		}
	}
}


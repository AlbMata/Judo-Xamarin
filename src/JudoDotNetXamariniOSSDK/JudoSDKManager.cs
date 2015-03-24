﻿using System;
using CoreLocation;
using System.Collections.Generic;
using UIKit;

namespace JudoDotNetXamariniOSSDK
{
	public class JudoSDKManager
	{
		Card currentCard {get; set;}
		CLLocation location {get; set;}
		Dictionary<string, string> clientDetails {get; set;}
		public static bool LocationEnabled{ get; set; }
		public static bool ThreeDSecureEnabled{ get; set; }
		public static bool AVSEnabled { get; set; }
		public static bool AmExAccepted { get; set; }
		public static bool MaestroAccepted { get; set; }

		private static readonly Lazy<JudoSDKManager> _singleton = new Lazy<JudoSDKManager>(() => new JudoSDKManager());

		public static JudoSDKManager Instance
		{
			get { return _singleton.Value; }
		}

		public static void SetSandboxMode()
		{
			
		}

		public static void SetProductionMode()
		{

		}

		public static void SetToken(string key, string secret)
		{

		}

		public static void SetOAuthToken(string oAuthToken)
		{

		}

		public static void EnableNavBar(bool navEnabled)
		{

		}

		public static void SetCurrency(string currency)
		{

		}

		public static void EnableFraudSignals(string deviceIdentifier)
		{

		}


		public static Dictionary<string, string> GetClientDetails(string deviceId)
		{
			
		}

		public static void SetUserAgent()
		{
			
		}

		public static bool ShouldCheckUserAgent()
		{

		}

		public static void HandleApplicationOpenURL(string url)
		{
			
		}

		public static CreditCardController GetCreditCardController()
		{

		}

		//TODO: correct the parameter for failureBlock action to be something meaningful instead of a string
		public static void MakeAPaymentCustomUI(decimal amount, string judoId, string paymentReference, string consumerReference, Dictionary<string, string> metaData, UIViewController viewController, 
								 Card card, UIViewController parentViewController, Action<string> successBlock, Action<string> failureBlock)
		{

		}

		public static void MakeAPayment(decimal amount, string judoId, string paymentReference, string consumerReference, Dictionary<string, string> metaData, 
									UIViewController parentViewController, Action<string> successBlock, Action<string> failureBlock)
		{

		}

		public static void MakeATokenPayment(decimal amount, Dictionary<string, string> cardDetails, string judoId, string paymentReference, string consumerReference, Dictionary<string, string> metaData, 
										UIViewController parentViewController, Action<string> successBlock, Action<string> failureBlock)
		{

		}

		public static void MakeAPreAuth(decimal amount, Dictionary<string, string> cardDetails, string judoId, string paymentReference, string consumerReference, Dictionary<string, string> metaData, UIViewController parentViewController, 
									Action<string> successBlock, Action<string> failureBlock)
		{
			
		}

		public static void MakeATokenPreAuth(decimal amount, Dictionary<string, string> cardDetails, string judoId, string paymentReference, string consumerReference, Dictionary<string, string> metaData, 
										UIViewController parentViewController, Action<string> successBlock, Action<string> failureBlock)
		{
			
		}

		public static void RegisterCard(Card card, string consumerReference, string deviceID, UIViewController parentViewController, Action<string> successBlock, Action<string> failureBlock)
		{
			
		}

		public void GetPath(string path, Dictionary<string, string> parameters, Action<string> successBlock, Action<string> failureBlock)
		{
			
		}

		public void PostPath(string path, Dictionary<string, string> parameters, Action<string> successBlock, Action<string> failureBlock)
		{

		}

		public void PutPath(string path, Dictionary<string, string> parameters, Action<string> successBlock, Action<string> failureBlock)
		{

		}
	}
}

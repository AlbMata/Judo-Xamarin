﻿using System;
using UIKit;
using Foundation;

using CoreGraphics;
using ObjCRuntime;
using System.Drawing;
using System.Collections.Generic;

namespace JudoDotNetXamariniOSSDK
{
	[Register ("SlideUpMenu")]
	public partial class SlideUpMenu :UIView
	{
		bool ComponentExpanded;
		UIPanGestureRecognizer panGesture;

		public SlideUpMenu (IntPtr p) : base (p)
		{
			
		}

		public SlideUpMenu (RectangleF frame) : base (frame)
		{
			this.Frame = frame;
		}

		public override void WillMoveToSuperview (UIView newsuper)
		{
			
			base.WillMoveToSuperview (newsuper);
			SetUpToggle (AVSSwitch, JudoSDKManager.AVSEnabled, () => {
				JudoSDKManager.AVSEnabled = !JudoSDKManager.AVSEnabled;
			});
			SetUpToggle (ThreeDSwitch, JudoSDKManager.ThreeDSecureEnabled, () => {
				JudoSDKManager.ThreeDSecureEnabled = !JudoSDKManager.ThreeDSecureEnabled;
			});
			SetUpToggle (RiskSwitch, JudoSDKManager.RiskSignals, () => {
				JudoSDKManager.RiskSignals = !JudoSDKManager.RiskSignals;
			});
			SetUpToggle (MaestroSwitch, JudoSDKManager.MaestroAccepted, () => {
				JudoSDKManager.MaestroAccepted = !JudoSDKManager.MaestroAccepted;
			});
			SetUpToggle (AmexSwitch, JudoSDKManager.AmExAccepted, () => {
				JudoSDKManager.AmExAccepted = !JudoSDKManager.AmExAccepted;
			});
			SetUpToggle (NoneUISwitch, !JudoSDKManager.UIMode, () => {
				JudoSDKManager.UIMode = !JudoSDKManager.UIMode;
				if (JudoSDKManager.UIMode)
					return;

				UIAlertView nonUIWarning = new UIAlertView ("Non-UI Mode",
					                              "You are about to use non UI Mode so please look at the source code to understand the usage of Non-UI APIs.",
					                              null, "OK", null);
				nonUIWarning.Show ();
			});
				
			AttachTouchEvents ();
			ArrowIcon.Transform = CGAffineTransform.MakeRotation ((nfloat)(180.0f * Math.PI) / 180.0f);

		}

		public override void WillRemoveSubview (UIView uiview)
		{
			base.WillRemoveSubview (uiview);
			AVSSwitch.ValueChanged -= delegate {
			};
			ThreeDSwitch.ValueChanged -= delegate {
			};
			RiskSwitch.ValueChanged -= delegate {
			};
			MaestroSwitch.ValueChanged -= delegate {
			};
			AmexSwitch.ValueChanged -= delegate {
			};
			NoneUISwitch.ValueChanged -= delegate {
			};

		}



		void SetUpToggle (UISwitch switchRef, bool CurrentValue, Action flipAction)
		{
			switchRef.Transform = CGAffineTransform.MakeScale (0.75F, 0.75F);
			switchRef.On = CurrentValue;
			switchRef.ValueChanged += delegate {
				flipAction ();
			};    
		}

		public override void AwakeFromNib ()
		{
			var arr = NSBundle.MainBundle.LoadNib ("SlideUpMenu", this, null);
			var v = Runtime.GetNSObject (arr.ValueAt (0)) as UIView;
			v.Frame = new CGRect (0, 0, Frame.Width, Frame.Height);
			AddSubview (v);
		}

		public override void Draw (CGRect rect)
		{
			base.Draw (rect);

		}

		void SlideAndFix (UIPanGestureRecognizer gesture)
		{

			UIView piece = gesture.View;
			nfloat yComponent = piece.Superview.Center.Y;
			if (!ComponentExpanded || piece.Frame.Top < piece.Superview.Center.Y) {
				yComponent = piece.Superview.Center.Y;
				ComponentExpanded = true;

			} else {
				yComponent = piece.Superview.Frame.Height - 40f;
				ComponentExpanded = false;
			}

			UIImageView.Animate (
				duration: 0.25f, 
				delay: 0,
				options: UIViewAnimationOptions.TransitionNone,
				animation: () => {
					piece.Frame = new CGRect (new CGPoint (piece.Frame.X, yComponent), piece.Frame.Size);

					gesture.SetTranslation (new CGPoint (0, 0), piece.Superview);
				},
				completion: () => {
					
				});			
		}

		public void ResetMenu ()
		{
			UIView piece = panGesture.View;
			var yComponent = piece.Superview.Frame.Height - 40f;
			ComponentExpanded = false;

			piece.Frame = new CGRect (new CGPoint (piece.Frame.X, yComponent), piece.Frame.Size);

			panGesture.SetTranslation (new CGPoint (0, 0), piece.Superview);

			this.Draw (this.Frame);
		}

		void PanGestureMoveAround (UIPanGestureRecognizer gesture)
		{
			
			UIView piece = gesture.View;

			AdjustAnchorPointForGestureRecognizer (gesture);

			if (gesture.State == UIGestureRecognizerState.Began || gesture.State == UIGestureRecognizerState.Changed) {

				CGPoint translation = gesture.TranslationInView (piece.Superview);

				piece.Center = new CGPoint (piece.Center.X, piece.Center.Y + translation.Y);
				 
				gesture.SetTranslation (new CGPoint (0, 0), piece.Superview);

			} else if (gesture.State == UIGestureRecognizerState.Ended) {
				SlideAndFix (gesture);
			} 		
		}

		void AdjustAnchorPointForGestureRecognizer (UIGestureRecognizer gestureRecognizer)
		{
			if (gestureRecognizer.State == UIGestureRecognizerState.Began) {
				UIView piece = gestureRecognizer.View;
				CGPoint locationInView = gestureRecognizer.LocationInView (piece);
				CGPoint locationInSuperview = gestureRecognizer.LocationInView (piece.Superview);

				piece.Layer.AnchorPoint = new CGPoint (locationInView.X / piece.Bounds.Size.Width, locationInView.Y / piece.Bounds.Size.Height);			
				piece.Center = locationInSuperview;
			}
		}

		private void AttachTouchEvents ()
		{

			panGesture = new UIPanGestureRecognizer ();

			panGesture.AddTarget (() => { 				
				PanGestureMoveAround (panGesture);
			});
			panGesture.MaximumNumberOfTouches = 2;
			this.AddGestureRecognizer (panGesture);
		}


		public void DrawBevel (RectangleF rect)
		{
			using (CGContext context = UIGraphics.GetCurrentContext ()) {

				context.SetLineWidth (1);
				UIColor.Black.SetFill ();
				UIColor.Black.SetStroke ();
				var currentPath = new CGPath ();
	
				currentPath.AddLines (new CGPoint[] {
					new CGPoint (10, 20),
					new CGPoint (16, 10), 
					new CGPoint (22, 20)
				});
				currentPath.CloseSubpath ();
				context.AddPath (currentPath);    
				context.DrawPath (CGPathDrawingMode.FillStroke);   
			}
		}



	}
}


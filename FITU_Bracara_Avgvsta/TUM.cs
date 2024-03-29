using Foundation;
using System;
using UIKit;
using CoreGraphics;

namespace FITU_Bracara_Avgvsta
{
	public partial class TUM : UIViewController
	{
		UIWebView webView; int executed = 0;string url = "http://ios.tum.pt/index.html";
		public TUM (IntPtr handle) : base (handle)
		{
			Title = NSBundle.MainBundle.LocalizedString ("TUM", "TUM");
			View.BackgroundColor = UIColor.White;
			webView = new UIWebView (View.Bounds);
			webView.ScrollView.ContentInset = new UIEdgeInsets(0,0,45,0);
			View.AddSubview(webView);
			url = "http://ios.tum.pt/index.html";
			webView.ScalesPageToFit = true;


			if(!Reachability.IsHostReachable("tum.pt")) {
				UIAlertView alert = new UIAlertView ();
				alert.Title = "Sem ligação à rede";
				alert.AddButton ("Continuar");
				alert.Message = "Não conseguirá usar a aplicação sem conexão à rede.";
				alert.Show ();
			}
			else
			{
				webView.LoadRequest(new NSUrlRequest(new NSUrl(url)));
			}

		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();

			// Release any cached data, images, etc that aren't in use.
		}

		#region View lifecycle

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			webView.ShouldStartLoad = HandleShouldStartLoad;
			Reachability.ReachabilityChanged += (sender, e) => {
				if (Reachability.InternetConnectionStatus() != NetworkStatus.NotReachable){
					webView.LoadRequest(new NSUrlRequest(new NSUrl(url)));
				}
			};

			// Perform any additional setup after loading the view, typically from a nib.
		}

		public override void ViewWillAppear (bool animated)
		{
			if (executed == 0) {
				executed = 1;
				base.ViewWillAppear (animated);
				int SystemVersion = Convert.ToInt16 (UIDevice.CurrentDevice.SystemVersion.Split ('.') [0].ToString ());
				if (SystemVersion >= 7) {
					this.EdgesForExtendedLayout = UIRectEdge.None;
					this.AutomaticallyAdjustsScrollViewInsets = false;
					this.ExtendedLayoutIncludesOpaqueBars = false;

					CGRect tempRect;

					foreach (UIView sub in this.View.Subviews) {
						tempRect = sub.Frame;
						tempRect.Y += 20.0f;
						sub.Frame = tempRect;
					}
				}
			}
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
		}


		bool HandleShouldStartLoad (UIWebView webView, NSUrlRequest request, UIWebViewNavigationType navigationType)
		{
			// Filter out clicked links
			if(navigationType == UIWebViewNavigationType.LinkClicked) {
				if(UIApplication.SharedApplication.CanOpenUrl(request.Url)) {
					// Open in Safari instead
					UIApplication.SharedApplication.OpenUrl(request.Url);
					return false;
				}
			}

			return true;
		}

		#endregion
	}
}



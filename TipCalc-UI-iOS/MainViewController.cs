using MonoTouch.UIKit;
using System;
using MonoTouch.Foundation;

using TipCalc.Util;

namespace TipCalcUIiOS
{
	public partial class MainViewController : UIViewController
	{
		UIPopoverController flipsidePopoverController;
		
		TipInfo info = new TipInfo ();
		
		public MainViewController (string nibName, NSBundle bundle) : base (nibName, bundle)
		{
			// Custom initialization
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			ScrollView.ContentSize = new System.Drawing.SizeF (Total.Frame.Width, Total.Frame.Height + Total.Frame.Top);
			
			Subtotal.EditingDidEnd += (sender, e) => {
				info.Subtotal = Parse (Subtotal);
				UpdateTipPercent (Parse (TipPercent));
				Subtotal.ResignFirstResponder ();
			};
			ReceiptTotal.EditingDidEnd += (sender, e) => {
				info.Total = Parse (ReceiptTotal);
				UpdateTipPercent (Parse (TipPercent));
				ReceiptTotal.ResignFirstResponder ();
			};
			TipPercent.EditingDidEnd += (sender, e) => {
				UpdateTipPercent (Parse (TipPercent));
			};
			TipPercentSlider.ValueChanged += (sender, e) => {
				TipPercent.Text = Math.Truncate(TipPercentSlider.Value).ToString ();
				UpdateTipPercent (Math.Truncate ((decimal) TipPercentSlider.Value));
			};
		}
		
		static decimal Parse (UITextField field)
		{
			try {
				return Convert.ToDecimal (field.Text);
			} catch (Exception e) {
				field.Text = e.Message;
				return 0m;
			}
		}

		void UpdateTipPercent (decimal newPercent)
		{
			info.TipPercent = newPercent;
			TipValue.Text = info.TipValue.ToString ();
			Total.Text = (info.Total + info.TipValue).ToString ();
		}

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone) {
				return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
			} else {
				return true;
			}
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			
			// Release any retained subviews of the main view.
			// e.g. this.myOutlet = null;
		}
		
		partial void showInfo (NSObject sender)
		{
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone) {
				var controller = new FlipsideViewController ("FlipsideViewController", null) {
					ModalTransitionStyle = UIModalTransitionStyle.FlipHorizontal,
				};
				controller.Done += delegate {
					this.DismissModalViewControllerAnimated (true);
				};
				this.PresentModalViewController (controller, true);
			} else {
				if (flipsidePopoverController == null) {
					var controller = new FlipsideViewController ("FlipsideViewController", null);
					flipsidePopoverController = new UIPopoverController (controller);
					controller.Done += delegate {
						flipsidePopoverController.Dismiss (true);
					};
				}
				if (flipsidePopoverController.PopoverVisible) {
					flipsidePopoverController.Dismiss (true);
				} else {
					flipsidePopoverController.PresentFromBarButtonItem ((UIBarButtonItem)sender, UIPopoverArrowDirection.Any, true);
				}
			}
		}
	}
}

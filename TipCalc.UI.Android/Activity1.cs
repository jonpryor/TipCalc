using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using TipCalc.Util;

namespace TipCalc.UI.Android
{
	[Activity (Label = "TipCalc.UI.Android", MainLauncher = true)]
	public class Activity1 : Activity
	{
		TipInfo info = new TipInfo () {
			TipPercent = 15,
		};

		TextView TipPercent;
		TextView Total;
		TextView TipValue;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			TipValue    = FindViewById<TextView>(Resource.Id.TipValue);
			Total       = FindViewById<TextView>(Resource.Id.Total);

			TipPercent = FindViewById<TextView>(Resource.Id.TipPercent);
			TipPercent.AfterTextChanged += (sender, e) => {
				UpdateTipPercent (Parse (TipPercent));
			};

			FindViewById<SeekBar>(Resource.Id.TipPercentSeekbar).SetOnSeekBarChangeListener (new SeekBarChangeListener (this));

			var subtotal = FindViewById<TextView>(Resource.Id.Subtotal);
			subtotal.AfterTextChanged += (sender, e) => {
				info.Subtotal = Parse (subtotal);
				UpdateTipPercent (info.TipPercent);
			};
			var total = FindViewById<TextView>(Resource.Id.ReceiptTotal);
			total.AfterTextChanged += (sender, e) => {
				info.Total = Parse (total);
				UpdateTipPercent (info.TipPercent);
			};
		}

		static decimal Parse (TextView field)
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

		class SeekBarChangeListener : Java.Lang.Object, SeekBar.IOnSeekBarChangeListener {

			Activity1 context;

			public SeekBarChangeListener (Activity1 context)
			{
				this.context = context;
			}

			public void OnProgressChanged (SeekBar seekBar, int progress, bool fromUser)
			{
				context.TipPercent.Text = progress.ToString ();
				context.UpdateTipPercent ((decimal) progress);
			}

			public void OnStartTrackingTouch (SeekBar seekBar)
			{
			}

			public void OnStopTrackingTouch (SeekBar seekBar)
			{
			}
		}
	}
}



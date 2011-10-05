using System;

namespace TipCalc.Util
{
	public class TipInfo
	{
		public decimal Total { get; set; }
		public decimal Subtotal { get; set; }
		public decimal TipPercent { get; set; }

		public decimal Tax {
			get {return Total - Subtotal;}
		}

		public decimal TipValue {
			get {
				if (Total == 0m || Subtotal == 0m || TipPercent == 0m)
					return 0m;

				var percent = TipPercent;
				percent /= 100;
				decimal value = (Subtotal * (1+percent)) + (Total - Subtotal);
				decimal fract = value - Math.Truncate (value);
				int f = (int) (fract * 100);
				while ((f % 25) != 0)
					++f;
				fract = f;
				fract /= 100;
				value = Math.Truncate (value) + fract;

				return value - Total;
			}
		}
	}
}


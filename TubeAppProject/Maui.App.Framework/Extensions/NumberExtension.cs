using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maui.App.Framework.Extensions
{
	public static class NumberExtension
	{
		public static string FormattedNumber(this double number)
		{
			return number switch
			{
				>= 1000000 => (number/1000000D).ToString("0.#M"),
				>= 10000 => (number/1000D).ToString("0.#k"),
				_ => number.ToString("#,0")
			};
		}
	}
}

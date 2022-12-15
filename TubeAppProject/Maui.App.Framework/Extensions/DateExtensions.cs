using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Maui.App.Framework.Extensions
{
	public static class DateExtensions
	{
		public static string ToTimeAgo(this DateTime baseTime)
		{ 
			var timeSpan = DateTime.Now - baseTime;

			if (timeSpan.TotalMinutes == 0)
				return "Just now";

			if (timeSpan.TotalMinutes == 1)
				return "1 min ago";

			if (timeSpan.TotalMinutes < 60 && timeSpan.TotalMinutes > 2)
				return Convert.ToInt32(timeSpan.TotalMinutes).ToString() + " mins ago";

			if (timeSpan.TotalHours < 2)
				return Convert.ToInt32(timeSpan.TotalHours).ToString() + " hour ago";

			if (timeSpan.TotalHours < 24)
				return Convert.ToInt32(timeSpan.TotalHours).ToString() + " hours ago";

			if (timeSpan.TotalDays < 2)
				return Convert.ToInt32(timeSpan.TotalDays).ToString() + " day ago";

			if (timeSpan.TotalDays < 24)
				return Convert.ToInt32(timeSpan.TotalDays).ToString() + " days ago";

			if (Convert.ToDouble(timeSpan.TotalDays / 365) < 2)
				return "1 year ago";

			return Convert.ToDouble(timeSpan.TotalDays / 365).ToString("#") + " years ago";

		}

		public static string ToReadableString(this TimeSpan span) =>
	   string.Format("{0}{1}{2}{3}",
		   span.Duration().Days > 0 ? string.Format("{0:0} day{1}, ", span.Days, span.Days == 1 ? string.Empty : "s") : string.Empty,
		   span.Duration().Hours > 0 ? string.Format("{0:0} hr{1}, ", span.Hours, span.Hours == 1 ? string.Empty : "s") : string.Empty,
		   span.Duration().Minutes > 0 ? string.Format("{0:0} min{1}, ", span.Minutes, span.Minutes == 1 ? string.Empty : "s") : string.Empty,
		   span.Duration().Seconds > 0 ? string.Format("{0:0} sec{1}", span.Seconds, span.Seconds == 1 ? string.Empty : "s") : string.Empty);

		public static TimeSpan ToTimeSpan(this string isoDuration) =>
			System.Xml.XmlConvert.ToTimeSpan(isoDuration);
	}


}

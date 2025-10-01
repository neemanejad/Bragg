using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Net.Mime.MediaTypeNames;

namespace Project.Utils
{
	internal class TimeUtils
	{
		public static String FormatSecondsToSurvivalTime(int seconds)
		{
			int formattedMinutes = GameManager.Instance.SurvivalTime / 60;
			int formattedSeconds = GameManager.Instance.SurvivalTime % 60;

			return $"{formattedMinutes:D2}:{formattedSeconds:D2}";
		}
	}
}
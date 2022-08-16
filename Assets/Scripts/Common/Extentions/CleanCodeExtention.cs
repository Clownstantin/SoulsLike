using System;

namespace SoulsLike.Extentions
{
	public static class CleanCodeExtention
	{
		public static T Do<T>(this T obj, Action<T> action) => Do(obj, action, true);

		public static T Do<T>(this T obj, Action<T> action, bool condition)
		{
			if(condition) action?.Invoke(obj);
			return obj;
		}
	}
}
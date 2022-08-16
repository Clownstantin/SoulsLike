using System;

namespace SoulsLike.Extentions
{
	public static class FunctionalExtention
	{
		public static T With<T>(this T self, Action<T> set)
		{
			set?.Invoke(self);
			return self;
		}

		public static T With<T>(this T self, Action<T> set, Func<bool> conditionFunc)
		{
			if(conditionFunc()) set?.Invoke(self);
			return self;
		}

		public static T With<T>(this T self, Action<T> set, bool condition)
		{
			if(condition) set?.Invoke(self);
			return self;
		}
	}
}
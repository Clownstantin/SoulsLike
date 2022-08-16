namespace SoulsLike
{
	public class Instance<T> where T : new()
	{
		public static T value = new();
	}
}
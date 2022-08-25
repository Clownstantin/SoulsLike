namespace SoulsLike
{
	public struct Landed : IGameEvent
	{
		public readonly bool isLongLand;

		public Landed(bool isLongLand) => this.isLongLand = isLongLand;
	}
}
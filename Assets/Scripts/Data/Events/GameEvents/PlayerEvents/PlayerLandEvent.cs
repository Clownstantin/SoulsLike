namespace SoulsLike
{
	public readonly struct PlayerLandEvent : IGameEvent
	{
		public readonly bool isLongLand;

		public PlayerLandEvent(bool isLongLand) => this.isLongLand = isLongLand;
	}
}
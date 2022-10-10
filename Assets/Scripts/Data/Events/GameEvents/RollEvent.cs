namespace SoulsLike
{
	public readonly struct RollEvent : IGameEvent
	{
		public readonly bool isMoving;

		public RollEvent(bool isMoving) => this.isMoving = isMoving;
	}
}
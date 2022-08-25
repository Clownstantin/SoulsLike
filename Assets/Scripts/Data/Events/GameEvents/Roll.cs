namespace SoulsLike
{
	public struct Roll : IGameEvent
	{
		public readonly bool isMoving;

		public Roll(bool isMoving) => this.isMoving = isMoving;
	}
}
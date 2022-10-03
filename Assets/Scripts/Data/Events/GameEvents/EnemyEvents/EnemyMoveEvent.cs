namespace SoulsLike
{
	public struct EnemyMoveEvent : IGameEvent
	{
		public readonly float delta;

		public EnemyMoveEvent(float delta) => this.delta = delta;
	}
}
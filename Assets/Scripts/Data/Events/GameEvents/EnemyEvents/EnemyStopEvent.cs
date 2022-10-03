namespace SoulsLike
{
	public struct EnemyStopEvent : IGameEvent
	{
		public readonly float delta;

		public EnemyStopEvent(float delta) => this.delta = delta;
	}
}
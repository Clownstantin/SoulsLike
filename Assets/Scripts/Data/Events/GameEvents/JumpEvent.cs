namespace SoulsLike
{
	public readonly struct JumpEvent : IGameEvent
	{
		public readonly float moveAmount;

		public JumpEvent(float moveAmount) => this.moveAmount = moveAmount;
	}
}
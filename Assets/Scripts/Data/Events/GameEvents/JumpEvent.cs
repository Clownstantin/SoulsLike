namespace SoulsLike
{
	public struct JumpEvent : IGameEvent
	{
		public readonly float moveAmount;

		public JumpEvent(float moveAmount) => this.moveAmount = moveAmount;
	}
}
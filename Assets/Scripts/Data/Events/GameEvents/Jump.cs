namespace SoulsLike
{
	public struct Jump : IGameEvent
	{
		public readonly float moveAmount;

		public Jump(float moveAmount) => this.moveAmount = moveAmount;
	}
}
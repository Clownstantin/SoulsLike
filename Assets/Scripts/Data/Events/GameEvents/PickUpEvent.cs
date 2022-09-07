namespace SoulsLike
{
	public struct PickUpEvent : IGameEvent
	{
		public readonly Item item;

		public PickUpEvent(Item item) => this.item = item;
	}
}
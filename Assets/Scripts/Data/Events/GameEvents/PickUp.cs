namespace SoulsLike
{
	public struct PickUp : IGameEvent
	{
		public readonly Item item;

		public PickUp(Item item) => this.item = item;
	}
}
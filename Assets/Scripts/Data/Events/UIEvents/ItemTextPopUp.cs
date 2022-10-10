namespace SoulsLike
{
	public readonly struct ItemTextPopUp : IGameEvent
	{
		public readonly Item item;
		public readonly bool isActive;

		public ItemTextPopUp(Item item, bool isActive)
		{
			this.item = item;
			this.isActive = isActive;
		}
	}
}
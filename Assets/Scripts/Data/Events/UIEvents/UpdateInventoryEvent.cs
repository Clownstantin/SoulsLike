using System.Collections.Generic;

namespace SoulsLike
{
	public struct UpdateInventoryEvent : IGameEvent
	{
		public readonly List<Item> itemInventory;

		public UpdateInventoryEvent(List<Item> itemInventory) => this.itemInventory = itemInventory;
	}
}
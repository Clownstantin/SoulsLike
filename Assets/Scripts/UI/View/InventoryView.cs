using System.Collections.Generic;
using SoulsLike.Extentions;
using UnityEngine;

namespace SoulsLike.UI
{
	[System.Serializable]
	public struct InventoryView : IView
	{
		[Header("Weapon Inventory")]
		[SerializeField] private WeaponInventorySlot _weaponSlotPrefab;
		[SerializeField] private RectTransform _weaponInventorySlotContainer;

		private WeaponInventorySlot[] _weaponInventorySlots;

		public void Init() => _weaponInventorySlots = _weaponInventorySlotContainer.GetComponentsInChildren<WeaponInventorySlot>();

		public void Subscribe() => this.AddListener<UpdateInventoryEvent>(OnInventoryUpdate);

		public void Unsubscribe() => this.RemoveListener<UpdateInventoryEvent>(OnInventoryUpdate);

		private void OnInventoryUpdate(UpdateInventoryEvent eventInfo)
		{
			List<Item> itemInventory = eventInfo.itemInventory;
			Log.Send($"Inventory Count {itemInventory.Count}");

			for(int i = 0; i < _weaponInventorySlots.Length; i++)
			{
				if(i < itemInventory.Count)
				{
					if(_weaponInventorySlots.Length < itemInventory.Count)
					{
						Object.Instantiate(_weaponSlotPrefab, _weaponInventorySlotContainer);
						_weaponInventorySlots = _weaponInventorySlotContainer.GetComponentsInChildren<WeaponInventorySlot>();
					}
					_weaponInventorySlots[i].AddItem(itemInventory[i]);
				}
				else
					_weaponInventorySlots[i].ClearInventorySlot();
			}
		}
	}
}

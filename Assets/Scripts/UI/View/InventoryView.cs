using System.Collections.Generic;
using SoulsLike.Extentions;
using UnityEngine;
using UnityEngine.UI;

namespace SoulsLike.UI
{
	[System.Serializable]
	public struct InventoryView : IView
	{
		[Header("Weapon Inventory")]
		[SerializeField] private WeaponInventorySlot _weaponSlotPrefab;
		[SerializeField] private RectTransform _weaponInventorySlotContainer;
		[Header("Equipment Menu")]
		[SerializeField] private RectTransform _equipmentSlotContainer;
		[SerializeField] private Button _leftSlotButton;
		[SerializeField] private Button _rightSlotButton;

		private WeaponInventorySlot[] _weaponInventorySlots;
		private HandWeaponSLot[] _handWeaponSlots;

		public void Init()
		{
			_weaponInventorySlots = _weaponInventorySlotContainer.GetComponentsInChildren<WeaponInventorySlot>();
			_handWeaponSlots = _equipmentSlotContainer.GetComponentsInChildren<HandWeaponSLot>();
		}

		public void Subscribe() => this.AddListener<UpdateInventoryEvent>(OnInventoryUpdate);

		public void Unsubscribe() => this.RemoveListener<UpdateInventoryEvent>(OnInventoryUpdate);

		private void OnInventoryUpdate(UpdateInventoryEvent eventInfo)
		{
			UpdateWeaponInventory(eventInfo.itemInventory);
			UpdateEquipment(eventInfo.rightHandWeapons, eventInfo.leftHandWeapons);
		}

		private void UpdateEquipment(WeaponItem[] rightWeapons, WeaponItem[] leftWeapons)
		{
			for(int i = 0; i < _handWeaponSlots.Length; i++)
			{
				if(_handWeaponSlots[i].IsLeftSlot01) _handWeaponSlots[i].AddItem(leftWeapons[0]);
				else if(_handWeaponSlots[i].IsLeftSlot02) _handWeaponSlots[i].AddItem(leftWeapons[1]);
				else if(_handWeaponSlots[i].IsRightSlot01) _handWeaponSlots[i].AddItem(rightWeapons[0]);
				else _handWeaponSlots[i].AddItem(rightWeapons[1]);
			}
		}

		private void UpdateWeaponInventory(List<Item> itemInventory)
		{
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

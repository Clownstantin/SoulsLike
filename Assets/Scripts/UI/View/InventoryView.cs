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
		[Header("Equipment Menu")]
		[SerializeField] private RectTransform _equipmentSlotContainer;

		private WeaponInventorySlot[] _weaponInventorySlots;
		private HandWeaponSlot[] _handWeaponSlots;

		public void Init()
		{
			_weaponInventorySlots = _weaponInventorySlotContainer.GetComponentsInChildren<WeaponInventorySlot>();
			_handWeaponSlots = _equipmentSlotContainer.GetComponentsInChildren<HandWeaponSlot>();
		}

		public void Subscribe()
		{
			this.AddListener<UpdateWeaponInventoryEvent>(OnInventoryUpdate);
			SubscribeButtonEvent(_handWeaponSlots);
			SubscribeButtonEvent(_weaponInventorySlots);
		}

		public void Unsubscribe()
		{
			this.RemoveListener<UpdateWeaponInventoryEvent>(OnInventoryUpdate);
			UnsubscribeButtonEvent(_handWeaponSlots);
			UnsubscribeButtonEvent(_weaponInventorySlots);
		}

		private void SubscribeButtonEvent(ISlot[] slot)
		{
			for(int i = 0; i < slot.Length; i++)
				slot[i].Subscribe();
		}

		private void UnsubscribeButtonEvent(ISlot[] slot)
		{
			for(int i = 0; i < slot.Length; i++)
				slot[i].Unsubscribe();
		}

		private void OnInventoryUpdate(UpdateWeaponInventoryEvent eventInfo)
		{
			UpdateWeaponInventory(eventInfo.itemInventory);
			UpdateEquipment(eventInfo.rightHandWeapons, eventInfo.leftHandWeapons);
		}

		private void UpdateEquipment(WeaponItem[] rightWeapons, WeaponItem[] leftWeapons)
		{
			for(int i = 0; i < _handWeaponSlots.Length; i++)
			{
				switch(_handWeaponSlots[i].SlotType)
				{
					case EquipmentSlotTypes.RightSlot01: _handWeaponSlots[i].AddItem(rightWeapons[0]); break;
					case EquipmentSlotTypes.RightSlot02: _handWeaponSlots[i].AddItem(rightWeapons[1]); break;
					case EquipmentSlotTypes.LeftSlot01: _handWeaponSlots[i].AddItem(leftWeapons[0]); break;
					case EquipmentSlotTypes.LeftSlot02: _handWeaponSlots[i].AddItem(leftWeapons[1]); break;
				}
			}
		}

		private void UpdateWeaponInventory(List<WeaponItem> weaponInventory)
		{
			for(int i = 0; i < _weaponInventorySlots.Length; i++)
			{
				if(i < weaponInventory.Count)
				{
					if(_weaponInventorySlots.Length < weaponInventory.Count)
					{
						WeaponInventorySlot weaponSlot = Object.Instantiate(_weaponSlotPrefab, _weaponInventorySlotContainer);
						weaponSlot.Subscribe();
						_weaponInventorySlots = _weaponInventorySlotContainer.GetComponentsInChildren<WeaponInventorySlot>();
					}
					_weaponInventorySlots[i].AddItem(weaponInventory[i]);
				}
				else
					_weaponInventorySlots[i].ClearInventorySlot();
			}
		}
	}
}

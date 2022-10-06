using System;
using System.Collections.Generic;
using SoulsLike.Extentions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SoulsLike.UI
{
	[Serializable]
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
			this.AddListener<OpenInventoryClick>(OnOpenInventoryClicked);
			SubscribeButtonEvent(_handWeaponSlots);
			SubscribeButtonEvent(_weaponInventorySlots);
		}

		public void Unsubscribe()
		{
			this.RemoveListener<UpdateWeaponInventoryEvent>(OnInventoryUpdate);
			this.RemoveListener<OpenInventoryClick>(OnOpenInventoryClicked);
			UnsubscribeButtonEvent(_handWeaponSlots);
			UnsubscribeButtonEvent(_weaponInventorySlots);
		}

		private void SubscribeButtonEvent(IEnumerable<ISlot> slots)
		{
			foreach(ISlot slot in slots)
				slot.Subscribe();
		}

		private void UnsubscribeButtonEvent(IEnumerable<ISlot> slots)
		{
			foreach(ISlot slot in slots)
				slot.Unsubscribe();
		}

		private void OnInventoryUpdate(UpdateWeaponInventoryEvent eventInfo)
		{
			UpdateWeaponInventory(eventInfo.itemInventory);
			UpdateEquipment(eventInfo.rightHandWeapons, eventInfo.leftHandWeapons);
		}

		private void OnOpenInventoryClicked(OpenInventoryClick _)
		{
			foreach(WeaponInventorySlot slot in _weaponInventorySlots)
				slot.DisableButton();
		}

		private void UpdateEquipment(IReadOnlyList<WeaponItem> rightWeapons, IReadOnlyList<WeaponItem> leftWeapons)
		{
			foreach(HandWeaponSlot slot in _handWeaponSlots)
			{
				switch(slot.SlotType)
				{
					case EquipmentSlotTypes.RightSlot01:
						slot.AddItem(rightWeapons[0]);
						break;
					case EquipmentSlotTypes.RightSlot02:
						slot.AddItem(rightWeapons[1]);
						break;
					case EquipmentSlotTypes.LeftSlot01:
						slot.AddItem(leftWeapons[0]);
						break;
					case EquipmentSlotTypes.LeftSlot02:
						slot.AddItem(leftWeapons[1]);
						break;
					default: return;
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
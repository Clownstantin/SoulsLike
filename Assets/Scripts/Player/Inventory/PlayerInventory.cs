using System.Collections.Generic;
using SoulsLike.Extentions;
using SoulsLike.UI;
using UnityEngine;

namespace SoulsLike.Player
{
	public class PlayerInventory : MonoBehaviour, IEventListener, IEventSender
	{
		[SerializeField] private WeaponItem _unarmedWeapon = default;
		[Header("Weapon Slots")]
		[SerializeField] private WeaponItem[] _rightHandWeapons = default;
		[SerializeField] private WeaponItem[] _leftHandWeapons = default;

		private List<WeaponItem> _weaponInventory = default;
		private WeaponItem _rightWeapon = default;
		private WeaponItem _leftWeapon = default;

		private int _currentRightWeaponIndex = default;
		private int _currentLeftWeaponIndex = default;

		public WeaponItem RightWeapon => _rightWeapon;
		public WeaponItem LeftWeapon => _leftWeapon;

		private void OnEnable()
		{
			this.AddListener<WeaponSwitchEvent>(OnWeaponSwitch);
			this.AddListener<PickUpEvent>(OnPickUp);
			this.AddListener<ToggleSelectionMenuEvent>(OnSelectionMenuToggle);
			this.AddListener<InventoryWeaponButtonClick>(OnInventoryWeaponButtonClicked);
		}

		private void OnDisable()
		{
			this.RemoveListener<WeaponSwitchEvent>(OnWeaponSwitch);
			this.RemoveListener<PickUpEvent>(OnPickUp);
			this.RemoveListener<ToggleSelectionMenuEvent>(OnSelectionMenuToggle);
			this.RemoveListener<InventoryWeaponButtonClick>(OnInventoryWeaponButtonClicked);
		}

		public void Init()
		{
			_weaponInventory = new List<WeaponItem>();
			_rightWeapon = _rightHandWeapons[0];
			_leftWeapon = _leftHandWeapons[0];

			this.TriggerEvent(new WeaponInitEvent(_rightWeapon, _leftWeapon));
			this.TriggerEvent(new UpdateWeaponInventoryEvent(_weaponInventory, _rightHandWeapons, _leftHandWeapons));

			UpdateWeaponsInHands();
		}

		private void OnWeaponSwitch(WeaponSwitchEvent eventInfo)
		{
			if(eventInfo.leftInput) HandleWeaponSwitch(ref _currentLeftWeaponIndex, out _leftWeapon, _leftHandWeapons, true);
			else if(eventInfo.rightInput) HandleWeaponSwitch(ref _currentRightWeaponIndex, out _rightWeapon, _rightHandWeapons);
		}

		private void OnPickUp(PickUpEvent eventInfo)
		{
			_weaponInventory.Add(eventInfo.weapon);
			this.TriggerEvent(new UpdateWeaponInventoryEvent(_weaponInventory, _rightHandWeapons, _leftHandWeapons));
		}

		private void HandleWeaponSwitch(ref int index, out WeaponItem weapon, IReadOnlyList<WeaponItem> targetWeapons, bool isLeft = false)
		{
			index++;

			if(index >= targetWeapons.Count)
			{
				index = -1;
				weapon = _unarmedWeapon;
			}
			else weapon = targetWeapons[index];

			this.TriggerEvent(new WeaponLoadEvent(weapon, isLeft));
		}

		private void OnSelectionMenuToggle(ToggleSelectionMenuEvent _) =>
			this.TriggerEvent(new UpdateWeaponInventoryEvent(_weaponInventory, _rightHandWeapons, _leftHandWeapons));

		private void OnInventoryWeaponButtonClicked(InventoryWeaponButtonClick eventInfo)
		{
			WeaponItem weapon = eventInfo.weapon;
			switch(eventInfo.slotType)
			{
				case EquipmentSlotTypes.RightSlot01: SwitchWeaponInHands(0, weapon, true); break;
				case EquipmentSlotTypes.RightSlot02: SwitchWeaponInHands(1, weapon, true); break;
				case EquipmentSlotTypes.LeftSlot01: SwitchWeaponInHands(0, weapon); break;
				case EquipmentSlotTypes.LeftSlot02: SwitchWeaponInHands(1, weapon); break;
			}

			_rightWeapon = _rightHandWeapons[_currentRightWeaponIndex];
			_leftWeapon = _leftHandWeapons[_currentLeftWeaponIndex];
			UpdateWeaponsInHands();

			this.TriggerEvent(new UpdateWeaponInventoryEvent(_weaponInventory, _rightHandWeapons, _leftHandWeapons));
		}

		private void SwitchWeaponInHands(int weaponIndex, WeaponItem weapon, bool isRightSlot = false)
		{
			WeaponItem[] weaponsInHands = isRightSlot ? _rightHandWeapons : _leftHandWeapons;
			_weaponInventory.Add(weaponsInHands[weaponIndex]);
			weaponsInHands[weaponIndex] = weapon;
			_weaponInventory.Remove(weapon);
		}

		private void UpdateWeaponsInHands()
		{
			this.TriggerEvent(new WeaponLoadEvent(_rightWeapon, false));
			this.TriggerEvent(new WeaponLoadEvent(_leftWeapon, true));
		}
	}
}
using System.Collections.Generic;
using SoulsLike.Extentions;
using UnityEngine;

namespace SoulsLike
{
	public class PlayerInventory : MonoBehaviour
	{
		[Header("Weapon on Start")]
		[SerializeField] private WeaponItem _rightWeapon = default;
		[SerializeField] private WeaponItem _leftWeapon = default;
		[SerializeField] private WeaponItem _unarmedWeapon = default;

		[Header("Weapon Slots")]
		[SerializeField] private WeaponItem[] _rightHandWeapons = default;
		[SerializeField] private WeaponItem[] _leftHandWeapons = default;

		private List<WeaponItem> _weaponInventory = default;

		private int _currentRightWeaponIndex = -1;
		private int _currentLeftWeaponIndex = -1;

		public WeaponItem RightWeapon => _rightWeapon;
		public WeaponItem LeftWeapon => _leftWeapon;

		private void OnEnable() => Subscribe();

		private void OnDisable() => Unsubscribe();

		public void Init()
		{
			_weaponInventory = new List<WeaponItem>();

			_rightWeapon = _rightHandWeapons[0];
			_leftWeapon = _leftHandWeapons[0];

			this.TriggerEvent(new WeaponInitEvent(_rightWeapon, _leftWeapon));
			this.TriggerEvent(new UpdateWeaponInventoryEvent(_weaponInventory, _rightHandWeapons, _leftHandWeapons));

			UpdateWeaponsInHands();
		}

		public void Subscribe()
		{
			this.AddListener<WeaponSwitchEvent>(OnWeaponSwitch);
			this.AddListener<PickUpWeaponEvent>(OnPickUp);
			this.AddListener<ToggleSelectionMenuEvent>(OnSelectionMenuToggle);
			this.AddListener<InventoryWeaponButtonClickEvent>(OnInventoryWeaponButtonClicked);
		}

		public void Unsubscribe()
		{
			this.RemoveListener<WeaponSwitchEvent>(OnWeaponSwitch);
			this.RemoveListener<PickUpWeaponEvent>(OnPickUp);
			this.RemoveListener<ToggleSelectionMenuEvent>(OnSelectionMenuToggle);
			this.RemoveListener<InventoryWeaponButtonClickEvent>(OnInventoryWeaponButtonClicked);
		}

		private void OnWeaponSwitch(WeaponSwitchEvent eventInfo)
		{
			if(eventInfo.leftInput) HandleWeaponSwitch(ref _currentLeftWeaponIndex, out _leftWeapon, _leftHandWeapons, true);
			else if(eventInfo.rightInput) HandleWeaponSwitch(ref _currentRightWeaponIndex, out _rightWeapon, _rightHandWeapons);
		}

		private void OnPickUp(PickUpWeaponEvent eventInfo)
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

		private void OnInventoryWeaponButtonClicked(InventoryWeaponButtonClickEvent eventInfo)
		{
			WeaponItem weapon = eventInfo.weapon;
			switch(eventInfo.slotType)
			{
				case UI.EquipmentSlotTypes.RightSlot01: SwitchWeaponInHands(0, weapon, true); break;
				case UI.EquipmentSlotTypes.RightSlot02: SwitchWeaponInHands(1, weapon, true); break;
				case UI.EquipmentSlotTypes.LeftSlot01: SwitchWeaponInHands(0, weapon); break;
				case UI.EquipmentSlotTypes.LeftSlot02: SwitchWeaponInHands(1, weapon); break;
			}

			_rightWeapon = _rightHandWeapons[0];
			_leftWeapon = _leftHandWeapons[0];
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

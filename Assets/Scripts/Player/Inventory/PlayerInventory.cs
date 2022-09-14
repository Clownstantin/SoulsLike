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

		private List<Item> _itemInventory = default;

		private int _currentRightWeaponIndex = -1;
		private int _currentLeftWeaponIndex = -1;

		public WeaponItem RightWeapon => _rightWeapon;
		public WeaponItem LeftWeapon => _leftWeapon;

		private void OnEnable()
		{
			this.AddListener<WeaponSwitchEvent>(ChangeWeaponInSlot);
			this.AddListener<PickUpEvent>(OnPickUp);
			this.AddListener<ToggleSelectionMenuEvent>(OnSelectionMenuToggle);
		}

		private void OnDisable()
		{
			this.RemoveListener<WeaponSwitchEvent>(ChangeWeaponInSlot);
			this.RemoveListener<PickUpEvent>(OnPickUp);
			this.RemoveListener<ToggleSelectionMenuEvent>(OnSelectionMenuToggle);
		}

		public void Init()
		{
			_itemInventory = new List<Item>();

			_rightWeapon = _unarmedWeapon;
			_leftWeapon = _unarmedWeapon;

			this.TriggerEvent(new WeaponInitEvent(_rightWeapon, _leftWeapon));
			this.TriggerEvent(new UpdateInventoryEvent(_itemInventory, _rightHandWeapons, _leftHandWeapons));
		}

		private void ChangeWeaponInSlot(WeaponSwitchEvent eventInfo)
		{
			if(eventInfo.leftInput) HandleWeaponSwitch(ref _currentLeftWeaponIndex, out _leftWeapon, _leftHandWeapons, true);
			else if(eventInfo.rightInput) HandleWeaponSwitch(ref _currentRightWeaponIndex, out _rightWeapon, _rightHandWeapons);
		}

		private void OnPickUp(PickUpEvent eventInfo) => _itemInventory.Add(eventInfo.item);

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
			this.TriggerEvent(new UpdateInventoryEvent(_itemInventory, _rightHandWeapons, _leftHandWeapons));
	}
}

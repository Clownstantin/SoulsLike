using System.Collections.Generic;
using UnityEngine;

namespace SoulsLike
{
	public class PlayerInventory : MonoBehaviour
	{
		[Header("Weapon on start")]
		[SerializeField] private WeaponItem _rightWeapon = default;
		[SerializeField] private WeaponItem _leftWeapon = default;
		[SerializeField] private WeaponItem _unarmedWeapon = default;

		[Header("Weapon slots")]
		[SerializeField] private WeaponItem[] _rightHandWeapons = default;
		[SerializeField] private WeaponItem[] _leftHandWeapons = default;

		private WeaponSlotManager _weaponSlotManager = default;
		private List<Item> _itemInventory = default;

		private int _currentRightWeaponIndex = -1;
		private int _currentLeftWeaponIndex = -1;

		public WeaponItem RightWeapon => _rightWeapon;
		public WeaponItem LeftWeapon => _leftWeapon;

		public void Init(WeaponSlotManager weaponSlotManager)
		{
			_weaponSlotManager = weaponSlotManager;

			_rightWeapon = _unarmedWeapon;
			_leftWeapon = _unarmedWeapon;
			_weaponSlotManager.LoadWeaponOnSlot(_rightWeapon);
			_weaponSlotManager.LoadWeaponOnSlot(_leftWeapon, true);

			_itemInventory = new List<Item>();
		}

		public void ChangeWeaponInSlot(bool isLeftSlot = false)
		{
			if(isLeftSlot) HandleWeaponLoad(ref _currentLeftWeaponIndex, ref _leftWeapon, _leftHandWeapons, isLeftSlot);
			else HandleWeaponLoad(ref _currentRightWeaponIndex, ref _rightWeapon, _rightHandWeapons);
		}

		public void AddItemToInventory(Item weaponItem) => _itemInventory.Add(weaponItem);

		private void HandleWeaponLoad(ref int index, ref WeaponItem weapon, WeaponItem[] targetWeapons, bool isLeft = false)
		{
			index++;

			if(index < targetWeapons.Length)
			{
				weapon = targetWeapons[index];
				_weaponSlotManager.LoadWeaponOnSlot(weapon, isLeft);
			}
			else
			{
				index = -1;
				weapon = _unarmedWeapon;
				_weaponSlotManager.LoadWeaponOnSlot(weapon, isLeft);
			}
		}
	}
}

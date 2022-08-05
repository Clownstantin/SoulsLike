using UnityEngine;

namespace SoulsLike
{
	public class PlayerInventory : MonoBehaviour
	{
		private WeaponSlotManager _weaponSlotManager;

		private int _currentRightWeaponIndex = -1;
		private int _currentLeftWeaponIndex = -1;

		public WeaponItem rightWeapon;
		public WeaponItem leftWeapon;
		public WeaponItem unarmedWeapon;

		public WeaponItem[] rightHandWeapons = new WeaponItem[1];
		public WeaponItem[] leftHandWeapons = new WeaponItem[1];

		public void Init(WeaponSlotManager weaponSlotManager)
		{
			_weaponSlotManager = weaponSlotManager;

			if(_weaponSlotManager)
			{
				rightWeapon = unarmedWeapon;
				leftWeapon = unarmedWeapon;
				_weaponSlotManager.LoadWeaponOnSlot(rightWeapon);
				_weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
			}
		}

		public void ChangeWeaponInSlot(bool isLeftSlot = false)
		{
			if(isLeftSlot) HandleWeaponLoad(ref _currentLeftWeaponIndex, ref leftWeapon, leftHandWeapons, isLeftSlot);
			else HandleWeaponLoad(ref _currentRightWeaponIndex, ref rightWeapon, rightHandWeapons);
		}

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
				weapon = unarmedWeapon;
				_weaponSlotManager.LoadWeaponOnSlot(weapon, isLeft);
			}
		}
	}
}

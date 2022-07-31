using UnityEngine;

namespace SoulsLike
{
	public class PlayerInventory : MonoBehaviour
	{
		private WeaponSlotManager _weaponSlotManager;

		public WeaponItem rightWeapon;
		public WeaponItem leftWeapon;

		public void Init(WeaponSlotManager weaponSlotManager)
		{
			_weaponSlotManager = weaponSlotManager;

			if(_weaponSlotManager)
			{
				_weaponSlotManager.LoadWeaponOnSlot(rightWeapon);
				_weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
			}
		}
	}
}

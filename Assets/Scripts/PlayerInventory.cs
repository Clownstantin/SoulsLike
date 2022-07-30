using UnityEngine;

namespace SoulsLike
{
	public class PlayerInventory : MonoBehaviour
	{
		private WeaponSlotManager _weaponSlotManager;

		public WeaponItem rightWeapon;
		public WeaponItem leftWeapon;

		#region MonoBehaviour
		private void Awake() => _weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();

		private void Start()
		{
			_weaponSlotManager.LoadWeaponOnSlot(rightWeapon);
			_weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
		}
		#endregion
	}
}

using UnityEngine;

namespace SoulsLike
{
	public class WeaponSlotManager : MonoBehaviour
	{
		private WeaponHolderSlot _leftHandSlot;
		private WeaponHolderSlot _rightHandSlot;

		#region MonoBehaviour
		private void Awake()
		{
			WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();

			foreach(WeaponHolderSlot weaponSlot in weaponHolderSlots)
			{
				if(weaponSlot.isLeftHandSlot) _leftHandSlot = weaponSlot;
				else if(weaponSlot.isRightHandSlot) _rightHandSlot = weaponSlot;
			}
		}
		#endregion

		public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft = default)
		{
			if(isLeft) _leftHandSlot.LoadWeaponModel(weaponItem);
			else _rightHandSlot.LoadWeaponModel(weaponItem);
		}
	}
}

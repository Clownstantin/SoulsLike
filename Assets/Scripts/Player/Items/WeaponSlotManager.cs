using UnityEngine;

namespace SoulsLike
{
	public class WeaponSlotManager : MonoBehaviour
	{
		private WeaponHolderSlot _leftHandSlot;
		private WeaponHolderSlot _rightHandSlot;

		private DamageDealer _leftDamageDialer;
		private DamageDealer _rightDamageDialer;

		public void Init()
		{
			WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();

			foreach(WeaponHolderSlot weaponSlot in weaponHolderSlots)
			{
				if(weaponSlot.isLeftHandSlot) _leftHandSlot = weaponSlot;
				else if(weaponSlot.isRightHandSlot) _rightHandSlot = weaponSlot;
			}
		}

		public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft = default)
		{
			if(isLeft)
			{
				_leftHandSlot.LoadWeaponModel(weaponItem);
				_leftDamageDialer = _leftHandSlot.currentWeaponModel.DamageDealer;
			}
			else
			{
				_rightHandSlot.LoadWeaponModel(weaponItem);
				_rightDamageDialer = _rightHandSlot.currentWeaponModel.DamageDealer;
			}
		}

		#region AnimationEvents
		public void OpenLeftDamageCollider() => _leftDamageDialer.EnableDamageCollider();

		public void OpenRightDamageCollider() => _rightDamageDialer.EnableDamageCollider();

		public void CloseLeftDamageCollider() => _leftDamageDialer.DisableDamageCollider();

		public void CloseRightDamageCollider() => _rightDamageDialer.DisableDamageCollider();
		#endregion
	}
}

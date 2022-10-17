using UnityEngine;

namespace SoulsLike.Enemy
{
	public class EnemyWeaponSlotManager : MonoBehaviour
	{
		[SerializeField] private WeaponItem _rightHandWeapon = default;
		[SerializeField] private WeaponItem _leftHandWeapon = default;

		private WeaponHolderSlot _leftHandSlot = default;
		private WeaponHolderSlot _rightHandSlot = default;

		private DamageDealer _leftDamageDialer = default;
		private DamageDealer _rightDamageDialer = default;

		public void Init()
		{
			WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();

			foreach(WeaponHolderSlot weaponSlot in weaponHolderSlots)
			{
				if(weaponSlot.IsLeftHandSlot) _leftHandSlot = weaponSlot;
				else if(weaponSlot.IsRightHandSlot) _rightHandSlot = weaponSlot;
			}

			LoadWeaponOnSlot(_rightHandWeapon, false);
			LoadWeaponOnSlot(_leftHandWeapon, true);
		}

		private void LoadWeaponOnSlot(WeaponItem weapon, bool isLeft)
		{
			if(!weapon) return;
			if(isLeft)
			{
				_leftHandSlot.LoadWeaponModel(weapon);
				_leftDamageDialer = _leftHandSlot.CurrentWeaponModel.DamageDealer;
			}
			else
			{
				_rightHandSlot.LoadWeaponModel(weapon);
				_rightDamageDialer = _rightHandSlot.CurrentWeaponModel.DamageDealer;
			}
		}

		#region AnimationEvents
		private void OpenDamageCollider() => _rightDamageDialer.EnableDamageCollider();

		private void CloseDamageCollider() => _rightDamageDialer.DisableDamageCollider();

		private void DrainStaminaOnLightAttack() { }

		private void DrainStaminaOnHeavyAttack() { }
		#endregion
	}
}
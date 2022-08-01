using UnityEngine;

namespace SoulsLike
{
	public class WeaponSlotManager : MonoBehaviour
	{
		private WeaponHolderSlot _leftHandSlot;
		private WeaponHolderSlot _rightHandSlot;

		private DamageDealer _leftDamageDialer;
		private DamageDealer _rightDamageDialer;

		private Animator _animator;

		public void Init(Animator animator)
		{
			_animator = animator;
			WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();

			foreach(WeaponHolderSlot weaponSlot in weaponHolderSlots)
			{
				if(weaponSlot.isLeftHandSlot) _leftHandSlot = weaponSlot;
				else if(weaponSlot.isRightHandSlot) _rightHandSlot = weaponSlot;
			}
		}

		public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft = default)
		{
			WeaponHolderSlot slot = isLeft ? _leftHandSlot : _rightHandSlot;
			string idleAnimationName = isLeft ? weaponItem.leftHand_Idle : weaponItem.rightHand_Idle;
			string emptyAnimationName = isLeft ? AnimationNameBase.LeftArmEmpty : AnimationNameBase.RightArmEmpty;

			slot.LoadWeaponModel(weaponItem);

			if(isLeft) _leftDamageDialer = _leftHandSlot.currentWeaponModel.DamageDealer;
			else _rightDamageDialer = _rightHandSlot.currentWeaponModel.DamageDealer;

			if(weaponItem) _animator.CrossFade(idleAnimationName, 0.2f);
			else _animator.CrossFade(emptyAnimationName, 0.2f);
		}

		#region AnimationEvents
		public void OpenLeftDamageCollider() => _leftDamageDialer.EnableDamageCollider();

		public void OpenRightDamageCollider() => _rightDamageDialer.EnableDamageCollider();

		public void CloseLeftDamageCollider() => _leftDamageDialer.DisableDamageCollider();

		public void CloseRightDamageCollider() => _rightDamageDialer.DisableDamageCollider();
		#endregion
	}
}

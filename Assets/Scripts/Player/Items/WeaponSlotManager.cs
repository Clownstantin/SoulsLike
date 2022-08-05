﻿using UnityEngine;

namespace SoulsLike
{
	public class WeaponSlotManager : MonoBehaviour
	{
		[SerializeField] private float _crossFadeTransitionDuration = 0.2f;

		private WeaponHolderSlot _leftHandSlot;
		private WeaponHolderSlot _rightHandSlot;

		private DamageDealer _leftDamageDialer;
		private DamageDealer _rightDamageDialer;

		private Animator _animator;
		private QuickSlotsUI _quickSlots;

		public void Init(Animator animator, QuickSlotsUI quickSlots)
		{
			_animator = animator;
			_quickSlots = quickSlots;

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
			_quickSlots.UpdateWeaponQuickSlotsUI(weaponItem, isLeft);

			if(isLeft) _leftDamageDialer = _leftHandSlot.currentWeaponModel.DamageDealer;
			else _rightDamageDialer = _rightHandSlot.currentWeaponModel.DamageDealer;

			if(weaponItem) _animator.CrossFade(idleAnimationName, _crossFadeTransitionDuration);
			else _animator.CrossFade(emptyAnimationName, _crossFadeTransitionDuration);
		}

		#region AnimationEvents
		public void OpenLeftDamageCollider() => _leftDamageDialer.EnableDamageCollider();

		public void OpenRightDamageCollider() => _rightDamageDialer.EnableDamageCollider();

		public void CloseLeftDamageCollider() => _leftDamageDialer.DisableDamageCollider();

		public void CloseRightDamageCollider() => _rightDamageDialer.DisableDamageCollider();
		#endregion
	}
}

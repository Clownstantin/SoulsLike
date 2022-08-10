using SoulsLike.Extentions;
using UnityEngine;

namespace SoulsLike
{
	public class WeaponSlotManager : MonoBehaviour
	{
		[SerializeField] private float _crossFadeTransitionDuration = 0.2f;

		private WeaponItem _attackingWeapon = default;

		private WeaponHolderSlot _leftHandSlot = default;
		private WeaponHolderSlot _rightHandSlot = default;

		private DamageDealer _leftDamageDialer = default;
		private DamageDealer _rightDamageDialer = default;

		private Animator _animator = default;
		private PlayerStats _playerStats = default;

		public void Init(Animator animator, PlayerStats playerStats)
		{
			_animator = animator;
			_playerStats = playerStats;

			WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();

			foreach(WeaponHolderSlot weaponSlot in weaponHolderSlots)
			{
				if(weaponSlot.IsLeftHandSlot) _leftHandSlot = weaponSlot;
				else if(weaponSlot.IsRightHandSlot) _rightHandSlot = weaponSlot;
			}
		}

		public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft = default)
		{
			WeaponHolderSlot slot = isLeft ? _leftHandSlot : _rightHandSlot;
			string handAnimationName = isLeft ? weaponItem.LeftHandAnimation : weaponItem.RightHandAnimation;
			string emptyAnimationName = isLeft ? AnimationNameBase.LeftArmEmpty : AnimationNameBase.RightArmEmpty;

			slot.LoadWeaponModel(weaponItem);

			if(isLeft)
			{
				this.TriggerEvent(EventID.OnLeftWeaponSwitch, weaponItem);
				_leftDamageDialer = _leftHandSlot.CurrentWeaponModel.DamageDealer;
			}
			else
			{
				this.TriggerEvent(EventID.OnRightWeaponSwitch, weaponItem);
				_rightDamageDialer = _rightHandSlot.CurrentWeaponModel.DamageDealer;
			}

			if(weaponItem) _animator.CrossFade(handAnimationName, _crossFadeTransitionDuration);
			else _animator.CrossFade(emptyAnimationName, _crossFadeTransitionDuration);
		}

		public void SetAttackingWeapon(WeaponItem weapon) => _attackingWeapon = weapon;

		#region AnimationEvents
		private void DrainStaminaOnLightAttack()
		{
			if(!_attackingWeapon) return;
			int drain = _attackingWeapon.BaseStamina * _attackingWeapon.LightAttackMultiplier;
			_playerStats.StaminaDrain(drain);
		}

		private void DrainStaminaOnHeavyAttack()
		{
			if(!_attackingWeapon) return;
			int drain = _attackingWeapon.BaseStamina * _attackingWeapon.HeavyAttackMultiplier;
			_playerStats.StaminaDrain(drain);
		}

		private void OpenLeftDamageCollider() => _leftDamageDialer.EnableDamageCollider();

		private void OpenRightDamageCollider() => _rightDamageDialer.EnableDamageCollider();

		private void CloseLeftDamageCollider() => _leftDamageDialer.DisableDamageCollider();

		private void CloseRightDamageCollider() => _rightDamageDialer.DisableDamageCollider();
		#endregion
	}
}

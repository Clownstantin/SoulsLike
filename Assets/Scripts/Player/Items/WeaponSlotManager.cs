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
		private IUnitStats _playerStats = default;

		private void OnEnable()
		{
			this.AddListener<WeaponInit>(OnWeaponInit);
			this.AddListener<WeaponLoad>(OnWeaponLoad);
		}

		private void OnDisable()
		{
			this.RemoveListener<WeaponInit>(OnWeaponInit);
			this.RemoveListener<WeaponLoad>(OnWeaponLoad);
		}

		public void Init(Animator animator, IUnitStats playerStats)
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

		private void OnWeaponInit(WeaponInit eventInfo)
		{
			_rightHandSlot.LoadWeaponModel(eventInfo.rightWeapon);
			_leftHandSlot.LoadWeaponModel(eventInfo.leftWeapon);

			_rightDamageDialer = _rightHandSlot.CurrentWeaponModel.DamageDealer;
			_leftDamageDialer = _leftHandSlot.CurrentWeaponModel.DamageDealer;

			_animator.CrossFade(eventInfo.rightWeapon.RightHandAnimation, _crossFadeTransitionDuration);
			_animator.CrossFade(eventInfo.leftWeapon.LeftHandAnimation, _crossFadeTransitionDuration);
		}

		private void OnWeaponLoad(WeaponLoad eventInfo)
		{
			bool isLeft = eventInfo.isLeft;
			WeaponItem weaponItem = eventInfo.weapon;

			WeaponHolderSlot slot = isLeft ? _leftHandSlot : _rightHandSlot;
			string weaponAnimationName = isLeft ? weaponItem.LeftHandAnimation : weaponItem.RightHandAnimation;
			string emptyAnimationName = isLeft ? AnimationNameBase.LeftArmEmpty : AnimationNameBase.RightArmEmpty;

			slot.LoadWeaponModel(weaponItem);

			if(isLeft) _leftDamageDialer = _leftHandSlot.CurrentWeaponModel.DamageDealer;
			else _rightDamageDialer = _rightHandSlot.CurrentWeaponModel.DamageDealer;

			_animator.CrossFade(weaponItem ? weaponAnimationName : emptyAnimationName, _crossFadeTransitionDuration);
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

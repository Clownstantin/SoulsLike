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
			this.AddListener(EventID.OnWeaponInit, OnWeaponInit);
			this.AddListener(EventID.OnWeaponLoad, OnWeaponLoad);
		}

		private void OnDisable()
		{
			this.RemoveListener(EventID.OnWeaponInit, OnWeaponInit);
			this.RemoveListener(EventID.OnWeaponLoad, OnWeaponLoad);
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

		private void InitWeapons((WeaponItem rightWeapon, WeaponItem leftWeapon) weapons)
		{
			_rightHandSlot.LoadWeaponModel(weapons.rightWeapon);
			_leftHandSlot.LoadWeaponModel(weapons.leftWeapon);

			_rightDamageDialer = _rightHandSlot.CurrentWeaponModel.DamageDealer;
			_leftDamageDialer = _leftHandSlot.CurrentWeaponModel.DamageDealer;

			_animator.CrossFade(weapons.rightWeapon.RightHandAnimation, _crossFadeTransitionDuration);
			_animator.CrossFade(weapons.leftWeapon.LeftHandAnimation, _crossFadeTransitionDuration);
		}

		private void LoadWeaponOnSlot((WeaponItem weapon, bool isLeft) values)
		{
			bool isLeft = values.isLeft;
			WeaponItem weaponItem = values.weapon;

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

		#region Actions
		private void OnWeaponInit(object weapons) => InitWeapons(((WeaponItem, WeaponItem))weapons);

		private void OnWeaponLoad(object weapon) => LoadWeaponOnSlot(((WeaponItem, bool))weapon);
		#endregion
	}
}

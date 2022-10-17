using SoulsLike.Extentions;
using SoulsLike.Player;
using UnityEngine;

namespace SoulsLike
{
	public class WeaponSlotManager : MonoBehaviour, IEventListener, IEventSender
	{
		private WeaponItem _attackingWeapon = default;

		private WeaponHolderSlot _leftHandSlot = default;
		private WeaponHolderSlot _rightHandSlot = default;
		private WeaponHolderSlot _backSlot = default;

		private DamageDealer _leftDamageDialer = default;
		private DamageDealer _rightDamageDialer = default;

		private PlayerAnimatorHandler _animatorHandler = default;

		private void OnEnable()
		{
			this.AddListener<WeaponInitEvent>(OnWeaponInit);
			this.AddListener<WeaponLoadEvent>(OnWeaponLoad);
		}

		private void OnDisable()
		{
			this.RemoveListener<WeaponInitEvent>(OnWeaponInit);
			this.RemoveListener<WeaponLoadEvent>(OnWeaponLoad);
		}

		public void Init(PlayerAnimatorHandler animatorHandler)
		{
			_animatorHandler = animatorHandler;
			WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();

			foreach(WeaponHolderSlot weaponSlot in weaponHolderSlots)
			{
				if(weaponSlot.IsLeftHandSlot) _leftHandSlot = weaponSlot;
				else if(weaponSlot.IsRightHandSlot) _rightHandSlot = weaponSlot;
				else if(weaponSlot.IsBackSlot) _backSlot = weaponSlot;
			}
		}

		public void SetAttackingWeapon(WeaponItem weapon) => _attackingWeapon = weapon;

		private void OnWeaponInit(WeaponInitEvent eventInfo)
		{
			_rightHandSlot.LoadWeaponModel(eventInfo.rightWeapon);
			_leftHandSlot.LoadWeaponModel(eventInfo.leftWeapon);

			_rightDamageDialer = _rightHandSlot.CurrentWeaponModel.DamageDealer;
			_leftDamageDialer = _leftHandSlot.CurrentWeaponModel.DamageDealer;
		}

		private void OnWeaponLoad(WeaponLoadEvent eventInfo)
		{
			bool isLeft = eventInfo.isLeftSlot;
			WeaponItem weapon = eventInfo.weapon;

			if(eventInfo.isBackSLot)
			{
				_backSlot.LoadWeaponModel(weapon);
				_leftHandSlot.DestroyWeapon();
				return;
			}
			_backSlot.DestroyWeapon();

			WeaponHolderSlot slot = isLeft ? _leftHandSlot : _rightHandSlot;
			slot.LoadWeaponModel(weapon);

			if(isLeft) _leftDamageDialer = _leftHandSlot.CurrentWeaponModel.DamageDealer;
			else _rightDamageDialer = _rightHandSlot.CurrentWeaponModel.DamageDealer;
		}

		#region AnimationEvents
		private void DrainStaminaOnLightAttack()
		{
			if(!_attackingWeapon) return;
			int drain = _attackingWeapon.BaseStamina * _attackingWeapon.LightAttackMultiplier;
			this.TriggerEvent(new StaminaDrain(drain));
		}

		private void DrainStaminaOnHeavyAttack()
		{
			if(!_attackingWeapon) return;
			int drain = _attackingWeapon.BaseStamina * _attackingWeapon.HeavyAttackMultiplier;
			this.TriggerEvent(new StaminaDrain(drain));
		}

		private void OpenDamageCollider()
		{
			DamageDealer dealer = _animatorHandler.IsUsingRightHand ? _rightDamageDialer : _leftDamageDialer;
			dealer.EnableDamageCollider();
		}

		private void CloseDamageCollider()
		{
			_rightDamageDialer.DisableDamageCollider();
			_leftDamageDialer.DisableDamageCollider();
		}
		#endregion
	}
}
using SoulsLike.Extentions;
using UnityEngine;

namespace SoulsLike
{
	public class WeaponSlotManager : MonoBehaviour
	{
		private WeaponItem _attackingWeapon = default;

		private WeaponHolderSlot _leftHandSlot = default;
		private WeaponHolderSlot _rightHandSlot = default;

		private DamageDealer _leftDamageDialer = default;
		private DamageDealer _rightDamageDialer = default;

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

		public void Init()
		{
			WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();

			foreach(WeaponHolderSlot weaponSlot in weaponHolderSlots)
			{
				if(weaponSlot.IsLeftHandSlot) _leftHandSlot = weaponSlot;
				else if(weaponSlot.IsRightHandSlot) _rightHandSlot = weaponSlot;
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
			bool isLeft = eventInfo.isLeft;
			WeaponItem weaponItem = eventInfo.weapon;

			WeaponHolderSlot slot = isLeft ? _leftHandSlot : _rightHandSlot;
			slot.LoadWeaponModel(weaponItem);

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

		private void OpenLeftDamageCollider() => _leftDamageDialer.EnableDamageCollider();

		private void OpenRightDamageCollider() => _rightDamageDialer.EnableDamageCollider();

		private void CloseLeftDamageCollider() => _leftDamageDialer.DisableDamageCollider();

		private void CloseRightDamageCollider() => _rightDamageDialer.DisableDamageCollider();
		#endregion
	}
}

using SoulsLike.Extentions;
using UnityEngine;

namespace SoulsLike
{
	public class PlayerAttackSystem : MonoBehaviour
	{
		private AnimatorHandler _animatorHandler = default;
		private WeaponSlotManager _weaponSlotManager = default;
		private PlayerInventory _playerInventory = default;

		private string _lastAttack = default;
		private bool _comboFlag = default;

		private void OnEnable() => this.AddListener<RightWeaponAttack>(OnRightWeaponAttack);

		private void OnDisable() => this.RemoveListener<RightWeaponAttack>(OnRightWeaponAttack);

		public void Init(PlayerInventory playerInventory, AnimatorHandler animatorHandler, WeaponSlotManager weaponSlotManager)
		{
			_playerInventory = playerInventory;
			_animatorHandler = animatorHandler;
			_weaponSlotManager = weaponSlotManager;
		}

		private void OnRightWeaponAttack(RightWeaponAttack eventInfo)
		{
			WeaponItem weapon = _playerInventory.RightWeapon;

			if(eventInfo.canDoCombo)
			{
				_comboFlag = true;
				HandleWeaponCombo(weapon);
				_comboFlag = false;
			}
			else
			{
				if(eventInfo.isInteracting) return;

				weapon.Do(HandleLightAttack, eventInfo.lightAttackInput)
					  .Do(HandleHeavyAttack, eventInfo.heavyAttackInput);
			}
		}

		private void HandleWeaponCombo(WeaponItem weapon)
		{
			if(!_comboFlag) return;

			_animatorHandler.DisableCombo();

			if(_lastAttack == weapon.OneHandedLightAttack01)
				_animatorHandler.PlayTargetAnimation(weapon.OneHandedLightAttack02, true);
			else if(_lastAttack == weapon.OneHandedHeavyAttack01)
				_animatorHandler.PlayTargetAnimation(weapon.OneHandedHeavyAttack02, true);
		}

		private void HandleLightAttack(WeaponItem weapon)
		{
			_weaponSlotManager.SetAttackingWeapon(weapon);
			_animatorHandler.PlayTargetAnimation(weapon.OneHandedLightAttack01, true);
			_lastAttack = weapon.OneHandedLightAttack01;
		}

		private void HandleHeavyAttack(WeaponItem weapon)
		{
			_weaponSlotManager.SetAttackingWeapon(weapon);
			_animatorHandler.PlayTargetAnimation(weapon.OneHandedHeavyAttack01, true);
			_lastAttack = weapon.OneHandedHeavyAttack01;
		}
	}
}

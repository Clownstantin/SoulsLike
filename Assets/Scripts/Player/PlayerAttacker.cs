using UnityEngine;

namespace SoulsLike
{
	public class PlayerAttacker : MonoBehaviour
	{
		private PlayerInputHandler _inputHandler = default;
		private AnimatorHandler _animatorHandler = default;
		private WeaponSlotManager _weaponSlotManager = default;

		private string _lastAttack = default;

		public void Init(PlayerInputHandler inputHandler, AnimatorHandler animatorHandler, WeaponSlotManager weaponSlotManager)
		{
			_inputHandler = inputHandler;
			_animatorHandler = animatorHandler;
			_weaponSlotManager = weaponSlotManager;
		}

		public void HandleWeaponCombo(WeaponItem weapon)
		{
			if(_inputHandler.ComboFlag)
			{
				_animatorHandler.DisableCombo();

				if(_lastAttack == weapon.OneHandedLightAttack_01)
					_animatorHandler.PlayTargetAnimation(weapon.OneHandedLightAttack_02, true);
				else if(_lastAttack == weapon.OneHandedHeavyAttack_01)
					_animatorHandler.PlayTargetAnimation(weapon.OneHandedHeavyAttack_02, true);
			}
		}

		public void HandleLightAttack(WeaponItem weapon)
		{
			_weaponSlotManager.SetAttackingWeapon(weapon);
			_animatorHandler.PlayTargetAnimation(weapon.OneHandedLightAttack_01, true);
			_lastAttack = weapon.OneHandedLightAttack_01;
		}

		public void HandleHeavyAttack(WeaponItem weapon)
		{
			_weaponSlotManager.SetAttackingWeapon(weapon);
			_animatorHandler.PlayTargetAnimation(weapon.OneHandedHeavyAttack_01, true);
			_lastAttack = weapon.OneHandedHeavyAttack_01;
		}
	}
}

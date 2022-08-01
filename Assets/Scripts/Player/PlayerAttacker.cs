using UnityEngine;

namespace SoulsLike
{
	public class PlayerAttacker : MonoBehaviour
	{
		private InputHandler _inputHandler;
		private AnimatorHandler _animatorHandler;

		public string lastAttack;

		public void Init(InputHandler inputHandler, AnimatorHandler animatorHandler)
		{
			_inputHandler = inputHandler;
			_animatorHandler = animatorHandler;
		}

		public void HandleWeaponCombo(WeaponItem weapon)
		{
			if(_inputHandler.comboFlag)
			{
				_animatorHandler.animator.SetBool(AnimatorHandler.CanDoCombo, false);

				if(lastAttack == weapon.OH_LightAttack_01)
					_animatorHandler.PlayTargetAnimation(weapon.OH_LightAttack_02, true);
				else if(lastAttack == weapon.OH_HeavyAttack_01)
					_animatorHandler.PlayTargetAnimation(weapon.OH_HeavyAttack_02, true);
			}
		}

		public void HandleLightAttack(WeaponItem weapon)
		{
			_animatorHandler.PlayTargetAnimation(weapon.OH_LightAttack_01, true);
			lastAttack = weapon.OH_LightAttack_01;
		}

		public void HandleHeavyAttack(WeaponItem weapon)
		{
			_animatorHandler.PlayTargetAnimation(weapon.OH_HeavyAttack_01, true);
			lastAttack = weapon.OH_HeavyAttack_01;
		}
	}
}

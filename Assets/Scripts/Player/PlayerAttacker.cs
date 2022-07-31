using UnityEngine;

namespace SoulsLike
{
	public class PlayerAttacker : MonoBehaviour
	{
		private AnimatorHandler _animatorHandler;

		public void Init(AnimatorHandler animatorHandler) => _animatorHandler = animatorHandler;

		public void HandleLightAttack(WeaponItem weapon) => _animatorHandler.PlayTargetAnimation(weapon.OHLightAttack, true);

		public void HandleHeavyAttack(WeaponItem weapon) => _animatorHandler.PlayTargetAnimation(weapon.OHHeavyAttack, true);
	}
}

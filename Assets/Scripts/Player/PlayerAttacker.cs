using UnityEngine;

namespace SoulsLike
{
	public class PlayerAttacker : MonoBehaviour
	{
		private AnimatorHandler _animatorHandler;

		#region MonoBehaviour
		private void Awake()
		{
			_animatorHandler = GetComponentInChildren<AnimatorHandler>();
		}
		#endregion

		public void HandleLightAttack(WeaponItem weapon)
		{
			_animatorHandler.PlayTargetAnimation(weapon.OHLightAttack, true);
		}

		public void HandleHeavyAttack(WeaponItem weapon)
		{
			_animatorHandler.PlayTargetAnimation(weapon.OHHeavyAttack, true);
		}
	}
}

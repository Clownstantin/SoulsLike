using SoulsLike.Extentions;
using UnityEngine;

namespace SoulsLike.Player
{
	[RequireComponent(typeof(Animator))]
	public class PlayerAnimatorHandler : AnimatorHandler, IEventListener, IEventSender
	{
		private bool _isInteracting = default;
		private bool _canDoCombo = default;

		public bool IsInteracting => _isInteracting;
		public bool CanDoCombo => _canDoCombo;

		private void OnEnable()
		{
			this.AddListener<PlayerHealthChanged>(OnHealthChangedAction);
			this.AddListener<PlayerDied>(OnPlayerDeathAction);
			this.AddListener<PlayerLandEvent>(OnLand);
			this.AddListener<PlayerFallEvent>(OnFall);
			this.AddListener<RollEvent>(OnRoll);
			this.AddListener<PickUpEvent>(OnPickUp);
			this.AddListener<JumpEvent>(OnJump);
			this.AddListener<WeaponInitEvent>(OnWeaponInit);
			this.AddListener<WeaponLoadEvent>(OnWeaponLoad);
			this.AddListener<GamePause>(OnGamePause);
			this.AddListener<GameResume>(OnGameResume);
		}

		private void OnDisable()
		{
			this.RemoveListener<PlayerHealthChanged>(OnHealthChangedAction);
			this.RemoveListener<PlayerDied>(OnPlayerDeathAction);
			this.RemoveListener<PlayerLandEvent>(OnLand);
			this.RemoveListener<PlayerFallEvent>(OnFall);
			this.RemoveListener<RollEvent>(OnRoll);
			this.RemoveListener<PickUpEvent>(OnPickUp);
			this.RemoveListener<WeaponInitEvent>(OnWeaponInit);
			this.RemoveListener<WeaponLoadEvent>(OnWeaponLoad);
			this.RemoveListener<JumpEvent>(OnJump);
			this.RemoveListener<GamePause>(OnGamePause);
			this.RemoveListener<GameResume>(OnGameResume);
		}

		private void OnAnimatorMove()
		{
			if(!_isInteracting) return;

			float delta = Time.deltaTime;

			Vector3 deltaPosition = animator.deltaPosition;
			deltaPosition.y = 0;
			Vector3 velocity = deltaPosition / delta;
			this.TriggerEvent(new PlayerAnimationPlay(velocity));
		}

		public void UpdateAnimatorValues(float delta, float verticalMovement, float horizontalMovement, bool isSprinting, bool isInAir)
		{
			_isInteracting = animator.GetBool(AnimatorParameterBase.IsInteracting);
			_canDoCombo = animator.GetBool(AnimatorParameterBase.CanDoCombo);

			float vertical = GetClampedAxis(verticalMovement);
			float horizontal = GetClampedAxis(horizontalMovement);

			if(isSprinting)
			{
				vertical = 2f;
				horizontal = horizontalMovement;
			}

			animator.SetFloat(verticalHash, vertical, 0.1f, delta);
			animator.SetFloat(horizontalHash, horizontal, 0.1f, delta);
			animator.SetBool(isInAirHash, isInAir);
		}

		private static float GetClampedAxis(float axis)
		{
			return axis switch
			{
				> 0 and < 0.55f => 0.5f,
				> 0.55f => 1f,
				< 0 and > -0.55f => -0.5f,
				< -0.55f => -1f,
				_ => 0
			};
		}

		#region AnimationEvents
		private void EnableCombo() => animator.SetBool(canDoComboHash, true);

		public void DisableCombo() => animator.SetBool(canDoComboHash, false);
		#endregion

		private void OnPlayerDeathAction(PlayerDied _) => PlayTargetAnimation(AnimationNameBase.Death, true);

		private void OnHealthChangedAction(PlayerHealthChanged _) => PlayTargetAnimation(AnimationNameBase.DamageTaken, true);

		private void OnFall(PlayerFallEvent _) => PlayTargetAnimation(AnimationNameBase.Fall, true);

		private void OnPickUp(PickUpEvent _) => PlayTargetAnimation(AnimationNameBase.PickUp, true);

		private void OnJump(JumpEvent _) => PlayTargetAnimation(AnimationNameBase.Jump, true);

		private void OnRoll(RollEvent eventInfo) =>
			PlayTargetAnimation(eventInfo.isMoving ? AnimationNameBase.Roll : AnimationNameBase.Stepback, eventInfo.isMoving);

		private void OnLand(PlayerLandEvent eventInfo) =>
			PlayTargetAnimation(eventInfo.isLongLand ? AnimationNameBase.Land : AnimationNameBase.Empty, eventInfo.isLongLand);

		private void OnGameResume(GameResume _) => animator.enabled = true;

		private void OnGamePause(GamePause _) => animator.enabled = false;

		private void OnWeaponInit(WeaponInitEvent eventInfo)
		{
			animator.CrossFade(eventInfo.rightWeapon.RightHandAnimation, crossFadeTransitionDuration);
			animator.CrossFade(eventInfo.leftWeapon.LeftHandAnimation, crossFadeTransitionDuration);
		}

		private void OnWeaponLoad(WeaponLoadEvent eventInfo)
		{
			bool isLeft = eventInfo.isLeftSlot;
			WeaponItem weapon = eventInfo.weapon;
			if(!weapon) return;

			string weaponAnimationName = isLeft ? weapon.LeftHandAnimation : weapon.RightHandAnimation;
			string emptyAnimationName = isLeft ? AnimationNameBase.LeftArmEmpty : AnimationNameBase.RightArmEmpty;

			animator.CrossFade(weapon ? weaponAnimationName : emptyAnimationName, crossFadeTransitionDuration);
		}
	}
}
using SoulsLike.Extentions;
using UnityEngine;

namespace SoulsLike
{
	[RequireComponent(typeof(Animator))]
	public class AnimatorHandler : MonoBehaviour
	{
		[SerializeField] private float _crossFadeTransitionDuration = 0.2f;

		private Animator _animator = default;

		private int _verticalHash = default;
		private int _horizontalHash = default;
		private int _isInteractingHash = default;
		private int _canDoComboHash = default;
		private int _isInAirHash = default;

		private bool _canRotate = default;
		private bool _isInteracting = default;
		private bool _canDoCombo = default;

		public bool CanRotate => _canRotate;
		public bool IsInteracting => _isInteracting;
		public bool CanDoCombo => _canDoCombo;

		private void OnEnable() => Subscribe();

		private void OnDisable() => Unsubscribe();

		private void OnAnimatorMove()
		{
			if(!_isInteracting) return;

			float delta = Time.deltaTime;

			Vector3 deltaPosition = _animator.deltaPosition;
			deltaPosition.y = 0;
			Vector3 velocity = deltaPosition / delta;
			this.TriggerEvent(new AnimationPlay(velocity));
		}

		public void Init()
		{
			_animator = GetComponent<Animator>();

			_verticalHash = Animator.StringToHash(AnimatorParameterBase.Vertical);
			_horizontalHash = Animator.StringToHash(AnimatorParameterBase.Horizontal);
			_isInteractingHash = Animator.StringToHash(AnimatorParameterBase.IsInteracting);
			_canDoComboHash = Animator.StringToHash(AnimatorParameterBase.CanDoCombo);
			_isInAirHash = Animator.StringToHash(AnimatorParameterBase.IsInAir);

			EnableRotation();
		}

		public void UpdateAnimatorValues(float delta, float verticalMovement, float horizontalMovement, bool isSprinting, bool isInAir)
		{
			_isInteracting = _animator.GetBool(AnimatorParameterBase.IsInteracting);
			_canDoCombo = _animator.GetBool(AnimatorParameterBase.CanDoCombo);

			float vertical = ClampAxis(verticalMovement);
			float horizontal = ClampAxis(horizontalMovement);

			if(isSprinting)
			{
				vertical = 2f;
				horizontal = horizontalMovement;
			}

			_animator.SetFloat(_verticalHash, vertical, 0.1f, delta);
			_animator.SetFloat(_horizontalHash, horizontal, 0.1f, delta);
			_animator.SetBool(_isInAirHash, isInAir);
		}

		public void PlayTargetAnimation(string animationName, bool isInteracting)
		{
			_animator.applyRootMotion = isInteracting;
			_animator.SetBool(_isInteractingHash, isInteracting);
			_animator.CrossFade(animationName, 0.2f);
		}

		private static float ClampAxis(float axis)
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
		private void EnableRotation() => _canRotate = true;

		private void StopRotation() => _canRotate = false;

		private void EnableCombo() => _animator.SetBool(_canDoComboHash, true);

		public void DisableCombo() => _animator.SetBool(_canDoComboHash, false);
		#endregion

		private void OnPlayerDeathAction(PlayerDied _) => PlayTargetAnimation(AnimationNameBase.Death, true);

		private void OnHealthChangedAction(HealthChanged _) => PlayTargetAnimation(AnimationNameBase.DamageTaken, true);

		private void OnFall(Fall _) => PlayTargetAnimation(AnimationNameBase.Fall, true);

		private void OnPickUp(PickUp _) => PlayTargetAnimation(AnimationNameBase.PickUp, true);

		private void OnJump(JumpEvent _) => PlayTargetAnimation(AnimationNameBase.Jump, true);

		private void OnRoll(Roll eventInfo) =>
			PlayTargetAnimation(eventInfo.isMoving ? AnimationNameBase.Roll : AnimationNameBase.Stepback, eventInfo.isMoving);

		private void OnLand(Landed eventInfo) =>
			PlayTargetAnimation(eventInfo.isLongLand ? AnimationNameBase.Land : AnimationNameBase.Empty, eventInfo.isLongLand);

		private void OnGameResume(GameResume _) => _animator.enabled = true;

		private void OnGamePause(GamePause _) => _animator.enabled = false;

		private void OnWeaponInit(WeaponInit eventInfo)
		{
			_animator.CrossFade(eventInfo.rightWeapon.RightHandAnimation, _crossFadeTransitionDuration);
			_animator.CrossFade(eventInfo.leftWeapon.LeftHandAnimation, _crossFadeTransitionDuration);
		}

		private void OnWeaponLoad(WeaponLoad eventInfo)
		{
			bool isLeft = eventInfo.isLeft;
			WeaponItem weaponItem = eventInfo.weapon;
			string weaponAnimationName = isLeft ? weaponItem.LeftHandAnimation : weaponItem.RightHandAnimation;
			string emptyAnimationName = isLeft ? AnimationNameBase.LeftArmEmpty : AnimationNameBase.RightArmEmpty;

			_animator.CrossFade(weaponItem ? weaponAnimationName : emptyAnimationName, _crossFadeTransitionDuration);
		}

		private void Subscribe()
		{
			this.AddListener<HealthChanged>(OnHealthChangedAction);
			this.AddListener<PlayerDied>(OnPlayerDeathAction);
			this.AddListener<Landed>(OnLand);
			this.AddListener<Fall>(OnFall);
			this.AddListener<Roll>(OnRoll);
			this.AddListener<PickUp>(OnPickUp);
			this.AddListener<JumpEvent>(OnJump);
			this.AddListener<WeaponInit>(OnWeaponInit);
			this.AddListener<WeaponLoad>(OnWeaponLoad);
			this.AddListener<GamePause>(OnGamePause);
			this.AddListener<GameResume>(OnGameResume);
		}

		private void Unsubscribe()
		{
			this.RemoveListener<HealthChanged>(OnHealthChangedAction);
			this.RemoveListener<PlayerDied>(OnPlayerDeathAction);
			this.RemoveListener<Landed>(OnLand);
			this.RemoveListener<Fall>(OnFall);
			this.RemoveListener<Roll>(OnRoll);
			this.RemoveListener<PickUp>(OnPickUp);
			this.RemoveListener<WeaponInit>(OnWeaponInit);
			this.RemoveListener<WeaponLoad>(OnWeaponLoad);
			this.RemoveListener<JumpEvent>(OnJump);
			this.RemoveListener<GamePause>(OnGamePause);
			this.RemoveListener<GameResume>(OnGameResume);
		}
	}
}

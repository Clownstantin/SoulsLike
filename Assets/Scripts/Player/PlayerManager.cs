﻿using UnityEngine;

namespace SoulsLike
{
	public class PlayerManager : UpdateableComponent
	{
		private Animator _animator = default;
		private AnimatorHandler _animatorHandler = default;
		private InputHandler _inputHandler = default;
		private CameraHandler _cameraHandler = default;

		private PlayerLocomotion _playerLocomotion = default;
		private PlayerStats _playerStats = default;
		private PlayerAttacker _playerAttacker = default;
		private PlayerInventory _playerInventory = default;

		private WeaponSlotManager _weaponSlotManager = default;
		private Transform _myTransform = default;

		private bool _isInteracting = default;
		private bool _canDoCombo = default;

		public bool IsInteracting => _isInteracting;
		public bool CanDoCombo => _canDoCombo;

		#region MonoBehaviour
		private void Awake()
		{
			_playerStats = GetComponent<PlayerStats>();
			_playerLocomotion = GetComponent<PlayerLocomotion>();
			_playerAttacker = GetComponent<PlayerAttacker>();
			_playerInventory = GetComponent<PlayerInventory>();
			_inputHandler = GetComponent<InputHandler>();

			_animator = GetComponentInChildren<Animator>();
			_animatorHandler = GetComponentInChildren<AnimatorHandler>();
			_weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();

			_weaponSlotManager.Init(_animator, _playerStats);
		}

		protected override void OnStart()
		{
			_myTransform = transform;
			_cameraHandler = GameManager.Instance.CameraHandler;

			_playerLocomotion.Init(this, _animatorHandler, _inputHandler);
			_playerAttacker.Init(_inputHandler, _animatorHandler, _weaponSlotManager);
			_playerInventory.Init(_weaponSlotManager);
			_inputHandler.Init(this, _playerAttacker, _playerInventory);
			_animatorHandler.Init(this, _playerLocomotion, _animator);
			_playerStats.Init(_animatorHandler);
		}

		public override void OnUpdate(float delta)
		{
			_isInteracting = _animator.GetBool(AnimatorHandler.IsInteracting);
			_canDoCombo = _animator.GetBool(AnimatorHandler.CanDoCombo);

			_inputHandler.TickInput(delta);

			_playerLocomotion.HandleMovement(delta);
			_playerLocomotion.HandleRollingAndSprinting();
			_playerLocomotion.HandleFalling(delta);

			CheckForInteractableObject();
		}

		public override void OnFixedUpdate(float delta)
		{
			if(_cameraHandler != null)
			{
				_cameraHandler.FollowTarget(delta, _myTransform.position);
				_cameraHandler.HandleCameraRotation(delta, _inputHandler.MouseX, _inputHandler.MouseY);
			}
		}

		public override void OnLateUpdate(float delta)
		{
			_inputHandler.ResetFlags();
			_playerLocomotion.HandleInAirTimer(delta);
		}
		#endregion

		private void CheckForInteractableObject()
		{
			LayerMask ignoreLayers = ~~(1 << 8 | 1 << 9 | 1 << 10);

			if(Physics.SphereCast(_myTransform.position, 0.3f, _myTransform.forward, out RaycastHit hit, ignoreLayers))
			{
				if(hit.collider.TryGetComponent(out Interactable interactableObj))
				{
					string interactableText = interactableObj.InteractableText;
					//UI pop up

					if(_inputHandler.InteractInput)
						interactableObj.PickUp(w => OnPickUp(w, interactableObj));
				}
			}
		}

		private void OnPickUp(Item weapon, Interactable interactableObj)
		{
			_playerLocomotion.rigidbody.velocity = Vector3.zero;
			_animatorHandler.PlayTargetAnimation(AnimationNameBase.PickUp, true);
			_playerInventory.AddItemToInventory(weapon);

			Destroy(interactableObj.gameObject);
		}
	}
}

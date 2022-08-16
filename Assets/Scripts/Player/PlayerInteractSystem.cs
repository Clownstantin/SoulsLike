using UnityEngine;

namespace SoulsLike
{
	public class PlayerInteractSystem : MonoBehaviour
	{
		[SerializeField] private float _checkRadius = default;
		[SerializeField] private float _checkDistance = default;

		private PlayerLocomotion _playerLocomotion = default;
		private AnimatorHandler _animatorHandler = default;
		private PlayerInventory _playerInventory = default;

		private Transform _myTransform = default;

		public void Init(AnimatorHandler animatorHandler, PlayerInventory playerInventory, PlayerLocomotion playerLocomotion)
		{
			_myTransform = transform;

			_animatorHandler = animatorHandler;
			_playerInventory = playerInventory;
			_playerLocomotion = playerLocomotion;
		}

		public void CheckObjectToInteract(bool interactInput)
		{
			if(!Physics.SphereCast(_myTransform.position, _checkRadius, _myTransform.forward,
				out RaycastHit hit, _checkDistance, _playerLocomotion.IgnoreForGroundCheck) ||
			   !hit.collider.TryGetComponent(out Interactable interactableObj)) return;

			string interactableText = interactableObj.InteractableText;
			//UI pop up

			if(interactInput) interactableObj.PickUp(OnPickUp);
		}

		private void OnPickUp(Item weapon)
		{
			_playerLocomotion.rigidbody.velocity = Vector3.zero;
			_animatorHandler.PlayTargetAnimation(AnimationNameBase.PickUp, true);
			_playerInventory.AddItemToInventory(weapon);
		}
	}
}

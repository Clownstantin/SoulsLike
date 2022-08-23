using SoulsLike.Extentions;
using UnityEngine;

namespace SoulsLike
{
	public class PlayerInteractSystem : MonoBehaviour
	{
		[SerializeField] private float _checkRadius = default;
		[SerializeField] private float _checkDistance = default;

		private AnimatorHandler _animatorHandler = default;
		private PlayerInventory _playerInventory = default;

		private Rigidbody _rigidbody = default;
		private Transform _myTransform = default;

		private LayerMask _ignoreForGroundCheck = default;

		public void Init(Rigidbody rigidbody, AnimatorHandler animatorHandler, PlayerInventory playerInventory)
		{
			_myTransform = transform;

			_rigidbody = rigidbody;
			_animatorHandler = animatorHandler;
			_playerInventory = playerInventory;

			_ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
		}

		public void CheckObjectToInteract(bool interactInput)
		{
			if(Physics.SphereCast(_myTransform.position, _checkRadius, _myTransform.forward,
				out RaycastHit hit, _checkDistance, _ignoreForGroundCheck) &&
			   hit.collider.TryGetComponent(out Interactable interactableObj))
			{
				string interactableText = interactableObj.InteractableText;

				this.TriggerEvent(new InteractTextPopUp(interactableText, true));

				if(interactInput) interactableObj.PickUp(OnPickUp);
			}
			else
			{
				this.TriggerEvent(new InteractTextPopUp(null, false));
				if(interactInput) this.TriggerEvent(new ItemTextPopUp(null, false));
			}
		}

		private void OnPickUp(Item weapon)
		{
			_rigidbody.velocity = Vector3.zero;
			_animatorHandler.PlayTargetAnimation(AnimationNameBase.PickUp, true);

			this.TriggerEvent(new ItemTextPopUp(weapon, true));
			_playerInventory.AddItemToInventory(weapon);
		}
	}
}

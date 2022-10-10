using SoulsLike.Extentions;
using UnityEngine;

namespace SoulsLike.Player
{
	public class PlayerInteractSystem : MonoBehaviour, IEventSender
	{
		[SerializeField] private float _checkRadius = default;
		[SerializeField] private float _checkDistance = default;

		private Transform _myTransform = default;
		private LayerMask _ignoreForGroundCheck = default;

		public void Init()
		{
			_myTransform = transform;
			_ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
		}

		public void CheckObjectToInteract(bool interactInput)
		{
			if(Physics.SphereCast(_myTransform.position, _checkRadius, _myTransform.forward, out RaycastHit hit, _checkDistance,
			                      _ignoreForGroundCheck) && hit.collider.TryGetComponent(out Interactable interactableObj))
			{
				string interactableText = interactableObj.InteractableText;
				this.TriggerEvent(new InteractTextPopUp(interactableText, true));
				if(interactInput) interactableObj.PickUp();
			}
			else
			{
				this.TriggerEvent(new InteractTextPopUp(null, false));
				if(interactInput) this.TriggerEvent(new ItemTextPopUp(null, false));
			}
		}
	}
}
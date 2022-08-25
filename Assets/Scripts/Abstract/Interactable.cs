﻿using SoulsLike.Extentions;
using UnityEngine;

namespace SoulsLike
{
	public abstract class Interactable : MonoBehaviour
	{
		[SerializeField] private float _radius = 0.6f;
		[SerializeField] private Vector3 _offset = default;
		[SerializeField] private string _interactableText = default;

		public string InteractableText => _interactableText;

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(transform.position + _offset, _radius);
		}

		public virtual void Interact() => this.TriggerEvent(new Interact());

		public virtual void PickUp() { }
	}
}

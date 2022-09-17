using UnityEngine;

namespace SoulsLike
{
	public abstract class UnitManager : UpdateableComponent
	{
		[SerializeField] private Transform _lockOnTransform = default;

		public Transform LockOnTransform => _lockOnTransform;
	}
}

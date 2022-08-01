using UnityEngine;

namespace SoulsLike
{
	public class DamageDealer : MonoBehaviour
	{
		[SerializeField] private int _currentWeaponDamage = 25;

		private Collider _damageCollider;

		private void Awake()
		{
			_damageCollider = GetComponent<Collider>();

			_damageCollider.gameObject.SetActive(true);
			_damageCollider.isTrigger = true;
			_damageCollider.enabled = false;
		}

		private void OnTriggerEnter(Collider other)
		{
			if(other.TryGetComponent(out Stats stats))
				stats.TakeDamage(_currentWeaponDamage);
		}

		public void EnableDamageCollider() => _damageCollider.enabled = true;

		public void DisableDamageCollider() => _damageCollider.enabled = false;
	}
}

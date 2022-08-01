using UnityEngine;

namespace SoulsLike
{
	public class DamagePlayer : MonoBehaviour
	{
		[SerializeField] private int _damage = 25;

		private void OnTriggerEnter(Collider other)
		{
			if(other.TryGetComponent(out Stats stats))
				stats.TakeDamage(_damage);
		}
	}
}

using UnityEngine;

namespace SoulsLike
{
	public class Weapon : MonoBehaviour
	{
		[SerializeField] private DamageDealer _damageDealer = default;

		public DamageDealer DamageDealer => _damageDealer;
	}
}

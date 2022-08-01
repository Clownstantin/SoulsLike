using UnityEngine;

namespace SoulsLike
{
	public abstract class Stats : MonoBehaviour
	{
		public int healthLevel = 10;
		public int maxHealth;
		public int currentHealth;

		public virtual void TakeDamage(int damage) => currentHealth -= damage;
	}
}

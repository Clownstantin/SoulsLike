using UnityEngine;

namespace SoulsLike
{
	public abstract class Stats : MonoBehaviour
	{
		[Header("Health")]
		[SerializeField] protected int healthLevel = 10;
		[SerializeField] protected int healthMultiplier = 10;

		[Header("Stamina")]
		[SerializeField] protected int staminaLevel = 10;
		[SerializeField] protected int staminaMultiplier = 10;

		protected int maxHealth = default;
		protected int currentHealth = default;

		protected int maxStamina = default;
		protected int currentStamina = default;

		public virtual void TakeDamage(int damage) => currentHealth -= damage;

		protected void SetStat(out int max, out int current, int level, int multiplier)
		{
			max = level * multiplier;
			current = max;
		}
	}
}

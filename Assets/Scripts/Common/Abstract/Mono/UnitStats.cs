using UnityEngine;

namespace SoulsLike
{
	public abstract class UnitStats : MonoBehaviour, IUnitStats
	{
		[SerializeField] protected UnitStatsData unitStatsData = default;
		protected bool isDead = default;

		public virtual void Init()
		{
			unitStatsData.maxHealth = unitStatsData.healthLevel * unitStatsData.healthMultiplier;
			unitStatsData.maxStamina = unitStatsData.staminaLevel * unitStatsData.staminaMultiplier;
			unitStatsData.currentHealth = unitStatsData.maxHealth;
			unitStatsData.currentStamina = unitStatsData.maxStamina;
		}

		public virtual void TakeDamage(int damage) => unitStatsData.currentHealth -= damage;
	}
}
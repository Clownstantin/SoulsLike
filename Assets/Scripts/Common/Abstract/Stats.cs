using UnityEngine;

namespace SoulsLike
{
	public abstract class Stats : MonoBehaviour, IUnitStats
	{
		[SerializeField] protected UnitData unitData = default;

		public virtual void TakeDamage(int damage) => unitData.currentHealth -= damage;

		protected void InitStats()
		{
			unitData.maxHealth = unitData.healthLevel * unitData.healthMultiplier;
			unitData.maxStamina = unitData.staminaLevel * unitData.staminaMultiplier;
			unitData.currentHealth = unitData.maxHealth;
			unitData.currentStamina = unitData.maxStamina;
		}
	}
}

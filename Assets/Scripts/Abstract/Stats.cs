using UnityEngine;

namespace SoulsLike
{
	public abstract class Stats : MonoBehaviour
	{
		[System.Serializable]
		public struct UnitData
		{
			public int healthLevel;
			public int healthMultiplier;
			public int staminaLevel;
			public int staminaMultiplier;

			[HideInInspector] public int maxHealth;
			[HideInInspector] public int currentHealth;
			[HideInInspector] public int maxStamina;
			[HideInInspector] public int currentStamina;
		}

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

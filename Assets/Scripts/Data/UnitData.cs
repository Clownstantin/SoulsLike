using UnityEngine;

namespace SoulsLike
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
}

using System;
using UnityEngine;

namespace SoulsLike
{
	[Serializable]
	public struct UnitStatsData
	{
		public int healthLevel;
		public int healthMultiplier;
		public int staminaLevel;
		public int staminaMultiplier;

		[HideInInspector] public int maxHealth;
		[HideInInspector] public int currentHealth;
		[HideInInspector] public int maxStamina;
		[HideInInspector] public float currentStamina;
	}
}
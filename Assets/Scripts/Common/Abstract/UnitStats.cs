using UnityEngine;

namespace SoulsLike
{
	public abstract class UnitStats : MonoBehaviour, IUnitStats
	{
		[SerializeField] protected UnitStatsData _unitStatsData = default;

		public virtual void TakeDamage(int damage) => _unitStatsData.currentHealth -= damage;

		protected void InitStats()
		{
			_unitStatsData.maxHealth = _unitStatsData.healthLevel * _unitStatsData.healthMultiplier;
			_unitStatsData.maxStamina = _unitStatsData.staminaLevel * _unitStatsData.staminaMultiplier;
			_unitStatsData.currentHealth = _unitStatsData.maxHealth;
			_unitStatsData.currentStamina = _unitStatsData.maxStamina;
		}
	}
}
using SoulsLike.Extentions;
using UnityEngine;

namespace SoulsLike.Player
{
	public class PlayerStats : UnitStats, IEventListener, IEventSender
	{
		[SerializeField] private float _staminaRegenAmount = default;

		private bool _isInvulnerable = default;

		private void OnEnable()
		{
			this.AddListener<StaminaDrain>(OnStaminaDrain);
			this.AddListener<PassPlayerAnimatorParams>(OnPassParams);
		}

		private void OnDisable()
		{
			this.RemoveListener<StaminaDrain>(OnStaminaDrain);
			this.RemoveListener<PassPlayerAnimatorParams>(OnPassParams);
		}

		public override void Init()
		{
			base.Init();
			this.TriggerEvent(new PlayerHealthInitEvent(unitStatsData.maxHealth));
			this.TriggerEvent(new StaminaInitEvent(unitStatsData.maxStamina));
		}

		public override void TakeDamage(int damage)
		{
			if(_isInvulnerable || isDead) return;
			base.TakeDamage(damage);
			this.TriggerEvent(new PlayerHealthChanged(unitStatsData.currentHealth));

			if(unitStatsData.currentHealth > 0) return;

			unitStatsData.currentHealth = 0;
			isDead = true;
			this.TriggerEvent(new PlayerDied());
		}

		public void RegenerateStamina(float delta, bool isInteracting)
		{
			if(unitStatsData.currentStamina >= unitStatsData.maxStamina) return;
			unitStatsData.currentStamina += _staminaRegenAmount * delta;
			this.TriggerEvent(new StaminaChanged(unitStatsData.currentStamina));
		}

		private void OnStaminaDrain(StaminaDrain eventInfo)
		{
			unitStatsData.currentStamina -= eventInfo.drainDamage;
			this.TriggerEvent(new StaminaChanged(unitStatsData.currentStamina));
		}

		private void OnPassParams(PassPlayerAnimatorParams eventInfo) => _isInvulnerable = eventInfo.isInvulnerable;
	}
}
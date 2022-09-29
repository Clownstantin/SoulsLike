using SoulsLike.Extentions;
using UnityEngine;
using UnityEngine.UI;

namespace SoulsLike.UI
{
	[System.Serializable]
	public struct BarsView : IView
	{
		[Header("Bars")]
		[SerializeField] private Slider _healthBarSlider;
		[SerializeField] private Slider _staminaBarSlider;

		public void Subscribe()
		{
			this.AddListener<HealthInitEvent>(OnHealthInit);
			this.AddListener<StaminaInitEvent>(OnStaminaInit);
			this.AddListener<HealthChanged>(OnHealthChanged);
			this.AddListener<StaminaChanged>(OnStaminaChanged);
		}

		public void Unsubscribe()
		{
			this.RemoveListener<HealthInitEvent>(OnHealthInit);
			this.RemoveListener<StaminaInitEvent>(OnStaminaInit);
			this.RemoveListener<HealthChanged>(OnHealthChanged);
			this.RemoveListener<StaminaChanged>(OnStaminaChanged);
		}

		private void OnHealthInit(HealthInitEvent eventInfo)
		{
			_healthBarSlider.maxValue = eventInfo.health;
			_healthBarSlider.value = eventInfo.health;
		}

		private void OnStaminaInit(StaminaInitEvent eventInfo)
		{
			_staminaBarSlider.maxValue = eventInfo.stamina;
			_staminaBarSlider.value = eventInfo.stamina;
		}

		private void OnHealthChanged(HealthChanged eventInfo) => _healthBarSlider.value = eventInfo.currentHealth;

		private void OnStaminaChanged(StaminaChanged eventInfo) => _staminaBarSlider.value = eventInfo.currentStamina;
	}
}

using SoulsLike.Extentions;
using UnityEngine;
using UnityEngine.UI;

namespace SoulsLike
{
	public class UIManager : MonoBehaviour
	{
		[Header("Bars")]
		[SerializeField] private Slider _healthBarSlider = default;
		[SerializeField] private Slider _staminaBarSlider = default;
		[Header("QuickSlots")]
		[SerializeField] private Image _leftWeaponIcon = default;
		[SerializeField] private Image _rightWeaponIcon = default;

		private void OnEnable() => Subscribe();

		private void OnDisable() => Unsubscribe();

		private static void SetWeaponSpriteToImage(Image image, Item weapon)
		{
			if(weapon.ItemIcon)
			{
				image.sprite = weapon.ItemIcon;
				image.enabled = true;
			}
			else
			{
				image.sprite = null;
				image.enabled = false;
			}
		}

		private void OnWeaponLoad(WeaponLoad eventInfo) =>
			SetWeaponSpriteToImage(eventInfo.isLeft ? _leftWeaponIcon : _rightWeaponIcon, eventInfo.weapon);

		private void OnHealthInit(HealthInit eventInfo)
		{
			_healthBarSlider.maxValue = eventInfo.health;
			_healthBarSlider.value = eventInfo.health;
		}

		private void OnStaminaInit(StaminaInit eventInfo)
		{
			_staminaBarSlider.maxValue = eventInfo.stamina;
			_staminaBarSlider.value = eventInfo.stamina;
		}

		private void OnHealthChanged(HealthChanged eventInfo) => _healthBarSlider.value = eventInfo.currentHealth;

		private void OnStaminaChanged(StaminaChanged eventInfo) => _staminaBarSlider.value = eventInfo.currentStamina;

		private void Subscribe()
		{
			this.AddListener<HealthInit>(OnHealthInit);
			this.AddListener<StaminaInit>(OnStaminaInit);
			this.AddListener<HealthChanged>(OnHealthChanged);
			this.AddListener<StaminaChanged>(OnStaminaChanged);
			this.AddListener<WeaponLoad>(OnWeaponLoad);
		}

		private void Unsubscribe()
		{
			this.RemoveListener<HealthInit>(OnHealthInit);
			this.RemoveListener<StaminaInit>(OnStaminaInit);
			this.RemoveListener<HealthChanged>(OnHealthChanged);
			this.RemoveListener<StaminaChanged>(OnStaminaChanged);
			this.RemoveListener<WeaponLoad>(OnWeaponLoad);
		}
	}
}

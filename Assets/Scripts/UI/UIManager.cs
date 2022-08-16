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

		private void LoadWeaponImage((WeaponItem weapon, bool isLeft) parameters) =>
			SetWeaponSpriteToImage(parameters.isLeft ? _leftWeaponIcon : _rightWeaponIcon, parameters.weapon);

		private void HealthSliderInit(int maxHealth)
		{
			_healthBarSlider.maxValue = maxHealth;
			_healthBarSlider.value = maxHealth;
		}

		private void StaminaSliderInit(int maxStamina)
		{
			_staminaBarSlider.maxValue = maxStamina;
			_staminaBarSlider.value = maxStamina;
		}

		private void SetHealth(int currentHealth) => _healthBarSlider.value = currentHealth;

		private void SetStamina(int currentStamina) => _staminaBarSlider.value = currentStamina;

		private void Subscribe()
		{
			this.AddListener(EventID.OnHealthInit, OnHealthInit);
			this.AddListener(EventID.OnStaminaInit, OnStaminaInit);
			this.AddListener(EventID.OnHealthChanged, OnHealthChanged);
			this.AddListener(EventID.OnStaminaChanged, OnStaminaChanged);
			this.AddListener(EventID.OnWeaponLoad, OnWeaponLoad);
		}

		private void Unsubscribe()
		{
			this.RemoveListener(EventID.OnHealthInit, OnHealthInit);
			this.RemoveListener(EventID.OnStaminaInit, OnStaminaInit);
			this.RemoveListener(EventID.OnHealthChanged, OnHealthChanged);
			this.RemoveListener(EventID.OnStaminaChanged, OnStaminaChanged);
			this.RemoveListener(EventID.OnWeaponLoad, OnWeaponLoad);
		}

		#region Actions
		private void OnHealthInit(object health) => HealthSliderInit((int)health);

		private void OnStaminaInit(object stamina) => StaminaSliderInit((int)stamina);

		private void OnHealthChanged(object health) => SetHealth((int)health);

		private void OnStaminaChanged(object stamina) => SetStamina((int)stamina);

		private void OnWeaponLoad(object weapon) => LoadWeaponImage(((WeaponItem, bool))weapon);
		#endregion
	}
}

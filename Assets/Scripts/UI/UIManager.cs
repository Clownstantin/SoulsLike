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

		private void OnWeaponSwitch(WeaponItem weapon, bool isLeft = false)
		{
			if(isLeft) SetWeaponSpriteToImage(_leftWeaponIcon, weapon);
			else SetWeaponSpriteToImage(_rightWeaponIcon, weapon);
		}

		private void SetWeaponSpriteToImage(Image image, WeaponItem weapon)
		{
			if(weapon.IsUnarmed)
			{
				image.sprite = null;
				image.enabled = false;
			}
			else
			{
				image.sprite = weapon.itemIcon;
				image.enabled = true;
			}
		}

		private void OnHealthInit(int maxHealth)
		{
			_healthBarSlider.maxValue = maxHealth;
			_healthBarSlider.value = maxHealth;
		}

		private void OnStaminaInit(int maxStamina)
		{
			_staminaBarSlider.maxValue = maxStamina;
			_staminaBarSlider.value = maxStamina;
		}

		private void OnHealthChanged(int currentHealth) => _healthBarSlider.value = currentHealth;

		private void OnStaminaChanged(int currentStamina) => _staminaBarSlider.value = currentStamina;

		private void Subscribe()
		{
			this.AddListener(EventID.OnHealthInit, h => OnHealthInit((int)h));
			this.AddListener(EventID.OnStaminaInit, s => OnStaminaInit((int)s));
			this.AddListener(EventID.OnHealthChanged, h => OnHealthChanged((int)h));
			this.AddListener(EventID.OnStaminaChanged, s => OnStaminaChanged((int)s));
			this.AddListener(EventID.OnLeftWeaponSwitch, w => OnWeaponSwitch((WeaponItem)w, true));
			this.AddListener(EventID.OnRightWeaponSwitch, w => OnWeaponSwitch((WeaponItem)w));
		}

		private void Unsubscribe()
		{
			this.RemoveListener(EventID.OnHealthInit, h => OnHealthInit((int)h));
			this.RemoveListener(EventID.OnStaminaInit, s => OnStaminaInit((int)s));
			this.RemoveListener(EventID.OnHealthChanged, h => OnHealthChanged((int)h));
			this.RemoveListener(EventID.OnStaminaChanged, s => OnStaminaChanged((int)s));
			this.RemoveListener(EventID.OnLeftWeaponSwitch, w => OnWeaponSwitch((WeaponItem)w, true));
			this.RemoveListener(EventID.OnRightWeaponSwitch, w => OnWeaponSwitch((WeaponItem)w));
		}
	}
}

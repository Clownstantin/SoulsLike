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

		public void UpdateWeaponQuickSlotsUI(WeaponItem weapon, bool isLeft = false)
		{
			if(isLeft) SetWeaponSpriteToImage(_leftWeaponIcon, weapon);
			else SetWeaponSpriteToImage(_rightWeaponIcon, weapon);
		}

		public void SetMaxHealth(int maxHealth)
		{
			_healthBarSlider.maxValue = maxHealth;
			_healthBarSlider.value = maxHealth;
		}

		public void SetMaxStamina(int maxStamina)
		{
			_staminaBarSlider.maxValue = maxStamina;
			_staminaBarSlider.value = maxStamina;
		}

		public void SetCurrentHealth(int currentHealth) => _healthBarSlider.value = currentHealth;

		public void SetCurrentStamina(int currentStamina) => _staminaBarSlider.value = currentStamina;

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
	}
}

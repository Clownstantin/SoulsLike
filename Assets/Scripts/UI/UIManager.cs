using SoulsLike.Extentions;
using TMPro;
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
		[Header("PopUp")]
		[SerializeField] private GameObject _interactPopUpContainer = default;
		[SerializeField] private GameObject _itemPopUpContainer = default;
		[SerializeField] private TMP_Text _interactText = default;
		[SerializeField] private TMP_Text _itemText = default;
		[SerializeField] private Image _itemImage = default;

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

		private void OnInteractTextPopUp(InteractTextPopUp eventInfo)
		{
			if(_interactPopUpContainer.activeSelf == eventInfo.isActive) return;
			_interactText.text = eventInfo.interactableText;
			_interactPopUpContainer.SetActive(eventInfo.isActive);
		}

		private void OnItemTextPopUp(ItemTextPopUp eventInfo)
		{
			if(_itemPopUpContainer.activeSelf == eventInfo.isActive) return;

			if(eventInfo.item)
			{
				_itemText.text = eventInfo.item.ItemName;
				_itemImage.sprite = eventInfo.item.ItemIcon;
			}

			_itemPopUpContainer.SetActive(eventInfo.isActive);
		}

		private void Subscribe()
		{
			this.AddListener<HealthInit>(OnHealthInit);
			this.AddListener<StaminaInit>(OnStaminaInit);
			this.AddListener<HealthChanged>(OnHealthChanged);
			this.AddListener<StaminaChanged>(OnStaminaChanged);
			this.AddListener<WeaponLoad>(OnWeaponLoad);
			this.AddListener<InteractTextPopUp>(OnInteractTextPopUp);
			this.AddListener<ItemTextPopUp>(OnItemTextPopUp);
		}

		private void Unsubscribe()
		{
			this.RemoveListener<HealthInit>(OnHealthInit);
			this.RemoveListener<StaminaInit>(OnStaminaInit);
			this.RemoveListener<HealthChanged>(OnHealthChanged);
			this.RemoveListener<StaminaChanged>(OnStaminaChanged);
			this.RemoveListener<WeaponLoad>(OnWeaponLoad);
			this.RemoveListener<InteractTextPopUp>(OnInteractTextPopUp);
			this.RemoveListener<ItemTextPopUp>(OnItemTextPopUp);
		}
	}
}

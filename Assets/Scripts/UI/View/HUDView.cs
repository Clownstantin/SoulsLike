using SoulsLike.Extentions;
using UnityEngine;
using UnityEngine.UI;

namespace SoulsLike.UI
{
	[System.Serializable]
	public struct HUDView : IView
	{
		[Header("Bars")]
		[SerializeField] private Slider _healthBarSlider;
		[SerializeField] private Slider _staminaBarSlider;
		[Header("QuickSlots")]
		[SerializeField] private Image _leftWeaponIcon;
		[SerializeField] private Image _rightWeaponIcon;
		[Header("Windows")]
		[SerializeField] private GameObject _hudWindow;
		[SerializeField] private GameObject _selectionWindow;
		[SerializeField] private GameObject _inventoryWindow;
		[SerializeField] private GameObject _equipmentWindow;
		[SerializeField] private bool _isSelectionMenuActive;

		public void Subscribe()
		{
			this.AddListener<HealthInit>(OnHealthInit);
			this.AddListener<StaminaInit>(OnStaminaInit);
			this.AddListener<HealthChanged>(OnHealthChanged);
			this.AddListener<StaminaChanged>(OnStaminaChanged);
			this.AddListener<WeaponLoadEvent>(OnWeaponLoad);
			this.AddListener<ToggleSelectionMenuEvent>(OnSelectionMenuToggle);
			this.AddListener<EquipButtonClickEvent>(OnEquipButtonClicked);
			this.AddListener<InventoryWeaponButtonClickEvent>(OnInventoryButtonClicked);
		}

		public void Unsubscribe()
		{
			this.RemoveListener<HealthInit>(OnHealthInit);
			this.RemoveListener<StaminaInit>(OnStaminaInit);
			this.RemoveListener<HealthChanged>(OnHealthChanged);
			this.RemoveListener<StaminaChanged>(OnStaminaChanged);
			this.RemoveListener<WeaponLoadEvent>(OnWeaponLoad);
			this.RemoveListener<ToggleSelectionMenuEvent>(OnSelectionMenuToggle);
			this.RemoveListener<EquipButtonClickEvent>(OnEquipButtonClicked);
			this.RemoveListener<InventoryWeaponButtonClickEvent>(OnInventoryButtonClicked);
		}

		private static void SetWeaponSpriteToImage(Image image, Item weapon, bool hasIcon)
		{
			image.sprite = weapon.ItemIcon;
			image.enabled = hasIcon;
		}

		private void OnWeaponLoad(WeaponLoadEvent eventInfo)
		{
			WeaponItem weapon = eventInfo.weapon;
			SetWeaponSpriteToImage(eventInfo.isLeft ? _leftWeaponIcon : _rightWeaponIcon, weapon, weapon.ItemIcon);
		}

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

		private void OnSelectionMenuToggle(ToggleSelectionMenuEvent _)
		{
			_isSelectionMenuActive = !_isSelectionMenuActive;
			_selectionWindow.SetActive(_isSelectionMenuActive);
			_hudWindow.SetActive(!_isSelectionMenuActive);

			if(!_isSelectionMenuActive)
			{
				_inventoryWindow.SetActive(false);
				_equipmentWindow.SetActive(false);
			}
		}

		private void OnEquipButtonClicked(EquipButtonClickEvent _)
		{
			_inventoryWindow.SetActive(true);
			_equipmentWindow.SetActive(false);
		}

		private void OnInventoryButtonClicked(InventoryWeaponButtonClickEvent _)
		{
			_inventoryWindow.SetActive(false);
			_equipmentWindow.SetActive(true);
		}
	}
}

using System;
using SoulsLike.Extentions;
using UnityEngine;
using UnityEngine.UI;

namespace SoulsLike.UI
{
	[Serializable]
	public struct HUDView : IView, IEventListener, IEventSender
	{
		[Header("QuickSlots")]
		[SerializeField] private Image _leftWeaponIcon;
		[SerializeField] private Image _rightWeaponIcon;

		[Header("Windows")]
		[SerializeField] private GameObject _hudWindow;
		[SerializeField] private GameObject _selectionWindow;
		[SerializeField] private GameObject _inventoryWindow;
		[SerializeField] private GameObject _equipmentWindow;
		[SerializeField] private bool _isSelectionMenuActive;

		[Header("Menu Buttons")]
		[SerializeField] private Button _openInventoryButton;
		[SerializeField] private Button _openEquipmentButton;
		[SerializeField] private Button _openOptionsButton;

		public void Subscribe()
		{
			this.AddListener<WeaponLoadEvent>(OnWeaponLoad);
			this.AddListener<ToggleSelectionMenuEvent>(OnSelectionMenuToggle);
			this.AddListener<EquipButtonClick>(OnEquipButtonClicked);
			this.AddListener<InventoryWeaponButtonClick>(OnInventoryButtonClicked);
			_openInventoryButton.onClick.AddListener(OnOpenInventoryClicked);
		}

		public void Unsubscribe()
		{
			this.RemoveListener<WeaponLoadEvent>(OnWeaponLoad);
			this.RemoveListener<ToggleSelectionMenuEvent>(OnSelectionMenuToggle);
			this.RemoveListener<EquipButtonClick>(OnEquipButtonClicked);
			this.RemoveListener<InventoryWeaponButtonClick>(OnInventoryButtonClicked);
			_openInventoryButton.onClick.RemoveListener(OnOpenInventoryClicked);
		}

		private static void SetWeaponSpriteToImage(Image image, Item weapon, bool hasIcon)
		{
			image.sprite = weapon.ItemIcon;
			image.enabled = hasIcon;
		}

		private void OnWeaponLoad(WeaponLoadEvent eventInfo)
		{
			WeaponItem weapon = eventInfo.weapon;
			SetWeaponSpriteToImage(eventInfo.isLeftSlot ? _leftWeaponIcon : _rightWeaponIcon, weapon, weapon.ItemIcon);
		}

		private void OnSelectionMenuToggle(ToggleSelectionMenuEvent _)
		{
			_isSelectionMenuActive = !_isSelectionMenuActive;
			_selectionWindow.SetActive(_isSelectionMenuActive);
			_hudWindow.SetActive(!_isSelectionMenuActive);

			if(_isSelectionMenuActive) return;
			_inventoryWindow.SetActive(false);
			_equipmentWindow.SetActive(false);
		}

		private void OnEquipButtonClicked(EquipButtonClick _)
		{
			_equipmentWindow.SetActive(false);
			_inventoryWindow.SetActive(true);
		}

		private void OnInventoryButtonClicked(InventoryWeaponButtonClick _)
		{
			_inventoryWindow.SetActive(false);
			_equipmentWindow.SetActive(true);
		}

		private void OnOpenInventoryClicked()
		{
			_selectionWindow.SetActive(false);
			_inventoryWindow.SetActive(true);
			this.TriggerEvent(new OpenInventoryClick());
		}
	}
}
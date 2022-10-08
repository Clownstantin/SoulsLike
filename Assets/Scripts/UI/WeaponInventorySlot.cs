using SoulsLike.Extentions;
using UnityEngine;
using UnityEngine.UI;

namespace SoulsLike.UI
{
	public class WeaponInventorySlot : MonoBehaviour, ISlot, IEventListener, IEventSender
	{
		[SerializeField] private Image _weaponIcon = default;
		[SerializeField] private Button _inventoryWeaponButton = default;

		private WeaponItem _weapon = default;
		private EquipmentSlotTypes _slotType = default;

		public void Subscribe()
		{
			_inventoryWeaponButton.onClick.AddListener(OnInventoryWeaponButtonClicked);
			this.AddListener<EquipButtonClick>(OnEquipButtonClicked);
		}

		public void Unsubscribe()
		{
			_inventoryWeaponButton.onClick.RemoveListener(OnInventoryWeaponButtonClicked);
			this.RemoveListener<EquipButtonClick>(OnEquipButtonClicked);
		}

		public void AddItem(WeaponItem weapon)
		{
			_weapon = weapon;
			_weaponIcon.sprite = _weapon.ItemIcon;
			_weaponIcon.enabled = true;
			gameObject.SetActive(true);
		}

		public void ClearInventorySlot()
		{
			_weapon = null;
			_weaponIcon.sprite = null;
			_weaponIcon.enabled = false;
			gameObject.SetActive(false);
		}

		public void DisableButton() => _inventoryWeaponButton.interactable = false;

		private void OnInventoryWeaponButtonClicked()
		{
			this.TriggerEvent(new InventoryWeaponButtonClick(_slotType, _weapon));
			_inventoryWeaponButton.interactable = false;
		}

		private void OnEquipButtonClicked(EquipButtonClick eventInfo)
		{
			_inventoryWeaponButton.interactable = true;
			_slotType = eventInfo.slotType;
		}
	}
}
using SoulsLike.Extentions;
using UnityEngine;
using UnityEngine.UI;

namespace SoulsLike.UI
{
	public class HandWeaponSlot : MonoBehaviour, ISlot
	{
		[SerializeField] private Image _weaponIcon = default;
		[SerializeField] private Button _equipButton = default;
		[SerializeField] private EquipmentSlotTypes _slotType = default;

		private WeaponItem _currentWeapon = default;

		public EquipmentSlotTypes SlotType => _slotType;

		public void Subscribe() => _equipButton.onClick.AddListener(OnEquipButtonClicked);

		public void Unsubscribe() => _equipButton.onClick.RemoveListener(OnEquipButtonClicked);

		public void AddItem(WeaponItem weapon)
		{
			_currentWeapon = weapon;
			_weaponIcon.sprite = _currentWeapon.ItemIcon;
			_weaponIcon.enabled = true;
			gameObject.SetActive(true);
		}

		public void ClearItem()
		{
			_currentWeapon = null;
			_weaponIcon.sprite = null;
			_weaponIcon.enabled = false;
			gameObject.SetActive(false);
		}

		private void OnEquipButtonClicked() => this.TriggerEvent(new EquipButtonClick(_slotType));
	}
}

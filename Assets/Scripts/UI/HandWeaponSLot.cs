using UnityEngine;
using UnityEngine.UI;

namespace SoulsLike.UI
{
	public class HandWeaponSLot : MonoBehaviour
	{
		[SerializeField] private Image _icon = default;
		[SerializeField] private bool _isRightSlot01 = default;
		[SerializeField] private bool _isRightSlot02 = default;
		[SerializeField] private bool _isLeftSlot01 = default;
		[SerializeField] private bool _isLeftSlot02 = default;

		public bool IsRightSlot01 => _isRightSlot01;
		public bool IsRightSlot02 => _isRightSlot02;
		public bool IsLeftSlot01 => _isLeftSlot01;
		public bool IsLeftSlot02 => _isLeftSlot02;

		private WeaponItem _currentWeapon = default;

		public void AddItem(WeaponItem weapon)
		{
			_currentWeapon = weapon;
			_icon.sprite = _currentWeapon.ItemIcon;
			_icon.enabled = true;
			gameObject.SetActive(true);
		}

		public void ClearItem()
		{
			_currentWeapon = null;
			_icon.sprite = null;
			_icon.enabled = false;
			gameObject.SetActive(false);
		}
	}
}

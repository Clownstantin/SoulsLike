using UnityEngine;
using UnityEngine.UI;

namespace SoulsLike.UI
{
	public class WeaponInventorySlot : MonoBehaviour
	{
		[SerializeField] private Image _weaponIcon = default;

		private Item _weaponItem = default;

		public void AddItem(Item weapon)
		{
			_weaponItem = weapon;
			_weaponIcon.sprite = _weaponItem.ItemIcon;
			_weaponIcon.enabled = true;
			gameObject.SetActive(true);
		}

		public void ClearInventorySlot()
		{
			_weaponItem = null;
			_weaponIcon.sprite = null;
			_weaponIcon.enabled = false;
			gameObject.SetActive(false);
		}
	}
}

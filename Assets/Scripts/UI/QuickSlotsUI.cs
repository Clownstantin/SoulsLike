using UnityEngine;
using UnityEngine.UI;

namespace SoulsLike
{
	public class QuickSlotsUI : MonoBehaviour
	{
		public Image leftWeaponIcon;
		public Image rightWeaponIcon;

		public void UpdateWeaponQuickSlotsUI(WeaponItem weapon, bool isLeft = false)
		{
			if(isLeft) SetWeaponSpriteToImage(leftWeaponIcon, weapon);
			else SetWeaponSpriteToImage(rightWeaponIcon, weapon);
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
	}
}

using UnityEngine;

namespace SoulsLike
{
	public class WeaponHolderSlot : MonoBehaviour
	{
		public Transform parentOverride;
		public bool isLeftHandSlot;
		public bool isRightHandSlot;

		[HideInInspector] public Weapon currentWeaponModel;

		public void LoadWeaponModel(WeaponItem weaponItem)
		{
			DestroyWeapon();

			if(!weaponItem)
			{
				UnloadWeapon();
				return;
			}

			Weapon weaponModel = Instantiate(weaponItem.WeaponPrefab);

			if(weaponModel)
			{
				if(parentOverride) weaponModel.transform.SetParent(parentOverride);
				else weaponModel.transform.SetParent(transform);

				weaponModel.transform.localPosition = Vector3.zero;
				weaponModel.transform.localRotation = Quaternion.identity;
				weaponModel.transform.localScale = Vector3.one;
			}

			currentWeaponModel = weaponModel;
		}

		public void DestroyWeapon()
		{
			if(currentWeaponModel) Destroy(currentWeaponModel);
		}

		public void UnloadWeapon()
		{
			if(currentWeaponModel) currentWeaponModel.gameObject.SetActive(false);
		}
	}
}

using UnityEngine;

namespace SoulsLike
{
	public class WeaponHolderSlot : MonoBehaviour
	{
		public Transform parentOverride;
		public bool isLeftHandSlot;
		public bool isRightHandSlot;

		public GameObject currentWeaponModel;

		public void LoadWeaponModel(WeaponItem weaponItem)
		{
			DestroyWeapon();

			if(!weaponItem)
			{
				UnloadWeapon();
				return;
			}

			GameObject weaponModel = Instantiate(weaponItem.weaponPrefab);

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
			if(currentWeaponModel) currentWeaponModel.SetActive(false);
		}
	}
}

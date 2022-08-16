using UnityEngine;

namespace SoulsLike
{
	public class WeaponHolderSlot : MonoBehaviour
	{
		[SerializeField] private Transform _parentOverride = default;
		[SerializeField] private bool _isLeftHandSlot = default;
		[SerializeField] private bool _isRightHandSlot = default;

		private Weapon _currentWeaponModel = default;

		public bool IsLeftHandSlot => _isLeftHandSlot;
		public bool IsRightHandSlot => _isRightHandSlot;
		public Weapon CurrentWeaponModel => _currentWeaponModel;

		public void LoadWeaponModel(WeaponItem weaponItem)
		{
			DestroyWeapon();

			if(!weaponItem)
			{
				UnloadWeapon();
				return;
			}

			Weapon weaponModel = Instantiate(weaponItem.WeaponPrefab);

			weaponModel.transform.SetParent(_parentOverride ? _parentOverride : transform);

			weaponModel.transform.localPosition = Vector3.zero;
			weaponModel.transform.localRotation = Quaternion.identity;
			weaponModel.transform.localScale = Vector3.one;

			_currentWeaponModel = weaponModel;
		}

		public void DestroyWeapon()
		{
			if(_currentWeaponModel) Destroy(_currentWeaponModel.gameObject);
		}

		public void UnloadWeapon()
		{
			if(_currentWeaponModel) _currentWeaponModel.gameObject.SetActive(false);
		}
	}
}

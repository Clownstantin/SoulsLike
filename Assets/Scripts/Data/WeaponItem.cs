using UnityEngine;

namespace SoulsLike
{
	[CreateAssetMenu(menuName = "Items/Weapon Item")]
	public class WeaponItem : Item
	{
		[SerializeField] private Weapon _weaponPrefab = default;
		[SerializeField] private bool _isUnarmed = default;

		public Weapon WeaponPrefab => _weaponPrefab;
		public bool IsUnarmed => _isUnarmed;

		[Header("One Handed Attack Animations")]
		public string OH_LightAttack_01 = AnimationNameBase.OH_LightAttack_01;
		public string OH_LightAttack_02 = AnimationNameBase.OH_LightAttack_02;
		public string OH_HeavyAttack_01 = AnimationNameBase.OH_HeavyAttack_01;
		public string OH_HeavyAttack_02 = AnimationNameBase.OH_HeavyAttack_02;
	}
}

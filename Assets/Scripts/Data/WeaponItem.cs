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

		[Header("Idle Animations")]
		public string rightHand_Idle = AnimationNameBase.RighArm_Idle;
		public string leftHand_Idle = AnimationNameBase.LeftArm_Idle;

		[Header("One Handed Attack Animations")]
		public string oneHandedLightAttack_01 = AnimationNameBase.OH_LightAttack_01;
		public string oneHandedLightAttack_02 = AnimationNameBase.OH_LightAttack_02;
		public string oneHandedHeavyAttack_01 = AnimationNameBase.OH_HeavyAttack_01;
		public string oneHandedHeavyAttack_02 = AnimationNameBase.OH_HeavyAttack_02;
	}
}

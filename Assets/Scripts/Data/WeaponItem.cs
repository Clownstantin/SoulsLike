using UnityEngine;

namespace SoulsLike
{
	[CreateAssetMenu(menuName = "Items/Weapon Item")]
	public class WeaponItem : Item
	{
		[SerializeField] private Weapon _weaponPrefab = default;
		[SerializeField] private bool _isUnarmed = default;

		[Header("Stamina Cost")]
		[SerializeField] private int _baseStamina = 20;
		[SerializeField] private int _lightAttackMultiplier = 1;
		[SerializeField] private int _heavyAttackMultiplier = 2;

		[Header("Idle Animations")]
		[SerializeField] private string _rightHandAnimation = AnimationNameBase.RighArm_Idle;
		[SerializeField] private string _leftHandAnimation = AnimationNameBase.LeftArm_Idle;

		[Header("One Handed Attack Animations")]
		[SerializeField] private string _oneHandedLightAttack_01 = AnimationNameBase.OH_LightAttack_01;
		[SerializeField] private string _oneHandedLightAttack_02 = AnimationNameBase.OH_LightAttack_02;
		[SerializeField] private string _oneHandedHeavyAttack_01 = AnimationNameBase.OH_HeavyAttack_01;
		[SerializeField] private string _oneHandedHeavyAttack_02 = AnimationNameBase.OH_HeavyAttack_02;

		public Weapon WeaponPrefab => _weaponPrefab;
		public bool IsUnarmed => _isUnarmed;

		public int BaseStamina => _baseStamina;
		public int LightAttackMultiplier => _lightAttackMultiplier;
		public int HeavyAttackMultiplier => _heavyAttackMultiplier;

		public string RightHandAnimation => _rightHandAnimation;
		public string LeftHandAnimation => _leftHandAnimation;

		public string OneHandedLightAttack_01 => _oneHandedLightAttack_01;
		public string OneHandedLightAttack_02 => _oneHandedLightAttack_02;
		public string OneHandedHeavyAttack_01 => _oneHandedHeavyAttack_01;
		public string OneHandedHeavyAttack_02 => _oneHandedHeavyAttack_02;
	}
}

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
		[SerializeField] private string _rightHandAnimation = AnimationNameBase.RighArmIdle;

		[SerializeField] private string _leftHandAnimation = AnimationNameBase.LeftArmIdle;

		[SerializeField] private string _twoHandAnimation = AnimationNameBase.TwoHandIdle;

		[Header("One Handed Attack Animations")]
		[SerializeField] private string _oneHandedLightAttack01 = AnimationNameBase.OhLightAttack01;

		[SerializeField] private string _oneHandedLightAttack02 = AnimationNameBase.OhLightAttack02;
		[SerializeField] private string _oneHandedHeavyAttack01 = AnimationNameBase.OhHeavyAttack01;
		[SerializeField] private string _oneHandedHeavyAttack02 = AnimationNameBase.OhHeavyAttack02;

		public Weapon WeaponPrefab => _weaponPrefab;
		public bool IsUnarmed => _isUnarmed;

		public int BaseStamina => _baseStamina;
		public int LightAttackMultiplier => _lightAttackMultiplier;
		public int HeavyAttackMultiplier => _heavyAttackMultiplier;

		public string RightHandAnimation => _rightHandAnimation;
		public string LeftHandAnimation => _leftHandAnimation;
		public string TwoHandAnimation => _twoHandAnimation;

		public string OneHandedLightAttack01 => _oneHandedLightAttack01;
		public string OneHandedLightAttack02 => _oneHandedLightAttack02;
		public string OneHandedHeavyAttack01 => _oneHandedHeavyAttack01;
		public string OneHandedHeavyAttack02 => _oneHandedHeavyAttack02;
	}
}

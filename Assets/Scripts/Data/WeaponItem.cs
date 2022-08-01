using UnityEngine;

namespace SoulsLike
{
	[CreateAssetMenu(menuName = "Items/Weapon Item")]
	public class WeaponItem : Item
	{
		public Weapon weaponPrefab = default;
		public bool isUnarmed = default;

		[Header("One Handed Attack Animations")]
		[HideInInspector] public string OHLightAttack = AnimationNameBase.OH_LightAttack_01;
		[HideInInspector] public string OHHeavyAttack = AnimationNameBase.OH_HeavyAttack_01;
	}
}

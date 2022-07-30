using UnityEngine;

namespace SoulsLike
{
	[CreateAssetMenu(menuName = "Items/Weapon Item")]
	public class WeaponItem : Item
	{
		public GameObject weaponPrefab = default;
		public bool isUnarmed = default;

		[Header("One Handed Attack Animations")]
		[HideInInspector] public string OHLightAttack = AnimatorHandler.OH_LightAttack_01;
		[HideInInspector] public string OHHeavyAttack = AnimatorHandler.OH_HeavyAttack_01;
	}
}

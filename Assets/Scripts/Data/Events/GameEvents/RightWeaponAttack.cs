namespace SoulsLike
{
	public struct RightWeaponAttack : IGameEvent
	{
		public readonly bool lightAttackInput;
		public readonly bool heavyAttackInput;
		public readonly bool isInteracting;
		public readonly bool canDoCombo;

		public RightWeaponAttack(bool lightAttackInput, bool heavyAttackInput, bool isInteracting, bool canDoCombo)
		{
			this.lightAttackInput = lightAttackInput;
			this.heavyAttackInput = heavyAttackInput;
			this.isInteracting = isInteracting;
			this.canDoCombo = canDoCombo;
		}
	}
}

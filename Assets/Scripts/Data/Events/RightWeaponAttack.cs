namespace SoulsLike
{
	public struct RightWeaponAttack : IGameEvent
	{
		public bool lightAttackInput;
		public bool heavyAttackInput;
		public bool isInteracting;
		public bool canDoCombo;

		public void Init(bool lightAttackInput,bool heavyAttackInput,bool isInteracting,bool canDocombo)
		{
			this.lightAttackInput = lightAttackInput;
			this.heavyAttackInput = heavyAttackInput;
			this.isInteracting = isInteracting;
			this.canDoCombo = canDocombo;
		}
	}
}
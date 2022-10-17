namespace SoulsLike
{
	public readonly struct PassPlayerAnimatorParams : IGameEvent
	{
		public readonly bool isInteracting;
		public readonly bool canDoCombo;
		public readonly bool isUsingRightHand;
		public readonly bool isInvulnerable;

		public PassPlayerAnimatorParams(bool isInteracting, bool canDoCombo, bool isUsingRightHand, bool isInvulnerable)
		{
			this.isInteracting = isInteracting;
			this.canDoCombo = canDoCombo;
			this.isUsingRightHand = isUsingRightHand;
			this.isInvulnerable = isInvulnerable;
		}
	}
}
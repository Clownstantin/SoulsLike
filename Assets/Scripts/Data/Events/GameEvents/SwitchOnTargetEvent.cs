namespace SoulsLike
{
	public struct SwitchOnTargetEvent : IGameEvent
	{
		public readonly bool isLeftTarget;
		public readonly bool isRightTarget;

		public SwitchOnTargetEvent(bool isLeftTarget, bool isRightTarget)
		{
			this.isLeftTarget = isLeftTarget;
			this.isRightTarget = isRightTarget;
		}
	}
}
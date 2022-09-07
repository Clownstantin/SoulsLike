namespace SoulsLike
{
	public struct WeaponSwitchEvent : IGameEvent
	{
		public readonly bool rightInput;
		public readonly bool leftInput;

		public WeaponSwitchEvent(bool rightInput, bool leftInput)
		{
			this.rightInput = rightInput;
			this.leftInput = leftInput;
		}
	}
}
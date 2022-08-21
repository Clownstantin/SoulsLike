namespace SoulsLike
{
	public struct WeaponSwitch : IGameEvent
	{
		public readonly bool rightInput;
		public readonly bool leftInput;

		public WeaponSwitch(bool rightInput, bool leftInput)
		{
			this.rightInput = rightInput;
			this.leftInput = leftInput;
		}
	}
}
namespace SoulsLike
{
	public struct WeaponSwitch : IGameEvent
	{
		public bool rightInput;
		public bool leftInput;

		public void Init(bool rightInput, bool leftInput)
		{
			this.rightInput = rightInput;
			this.leftInput = leftInput;
		}
	}
}
namespace SoulsLike
{
	public struct StaminaInit : IGameEvent
	{
		public readonly int stamina;

		public StaminaInit(int stamina) => this.stamina = stamina;
	}
}
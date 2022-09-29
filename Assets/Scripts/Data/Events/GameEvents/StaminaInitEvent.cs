namespace SoulsLike
{
	public struct StaminaInitEvent : IGameEvent
	{
		public readonly int stamina;

		public StaminaInitEvent(int stamina) => this.stamina = stamina;
	}
}
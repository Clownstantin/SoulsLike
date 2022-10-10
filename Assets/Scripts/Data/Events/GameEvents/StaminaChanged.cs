namespace SoulsLike
{
	public readonly struct StaminaChanged : IGameEvent
	{
		public readonly int currentStamina;

		public StaminaChanged(int stamina) => currentStamina = stamina;
	}
}
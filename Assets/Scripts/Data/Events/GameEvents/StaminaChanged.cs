namespace SoulsLike
{
	public readonly struct StaminaChanged : IGameEvent
	{
		public readonly float currentStamina;

		public StaminaChanged(float stamina) => currentStamina = stamina;
	}
}
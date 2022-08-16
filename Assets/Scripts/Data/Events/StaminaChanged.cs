namespace SoulsLike
{
	public struct StaminaChanged : IGameEvent
	{
		public int currentStamina;

		public void Init(int stamina) => currentStamina = stamina;
	}
}
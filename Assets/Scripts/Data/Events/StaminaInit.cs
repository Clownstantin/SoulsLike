namespace SoulsLike
{
	public struct StaminaInit : IGameEvent
	{
		public int stamina;

		public void Init(int stamina) => this.stamina = stamina;
	}
}
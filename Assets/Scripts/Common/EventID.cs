namespace SoulsLike
{
	public enum EventID
	{
		None = 0,
		OnHealthInit,
		OnStaminaInit,
		OnHealthChanged,
		OnStaminaChanged,
		OnWeaponSwitch,
		OnLeftWeaponAttack,
		OnWeaponInit,
		OnWeaponLoad,
		OnRightWeaponAttack,
		OnRightWeaponLoad,
		OnPickUp,
		OnPlayerDeath,
		OnEnemyDeath,
		OnGamePause,
		OnGameResume,
		Length
	}
}

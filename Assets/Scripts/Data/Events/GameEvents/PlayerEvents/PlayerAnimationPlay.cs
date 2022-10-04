using UnityEngine;

namespace SoulsLike
{
	public struct PlayerAnimationPlay : IGameEvent
	{
		public readonly Vector3 velocity;

		public PlayerAnimationPlay(Vector3 velocity) => this.velocity = velocity;
	}
}
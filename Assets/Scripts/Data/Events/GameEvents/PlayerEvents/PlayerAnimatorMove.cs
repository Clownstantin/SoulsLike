using UnityEngine;

namespace SoulsLike
{
	public readonly struct PlayerAnimatorMove : IGameEvent
	{
		public readonly Vector3 velocity;

		public PlayerAnimatorMove(Vector3 velocity) => this.velocity = velocity;
	}
}
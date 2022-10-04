using UnityEngine;

namespace SoulsLike
{
	public struct EnemyAnimationPlay : IGameEvent
	{
		public readonly Vector3 velocity;

		public EnemyAnimationPlay(Vector3 velocity) => this.velocity = velocity;
	}
}
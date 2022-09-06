using UnityEngine;

namespace SoulsLike
{
	public struct AnimationPlay : IGameEvent
	{
		public readonly Vector3 velocity;

		public AnimationPlay(Vector3 velocity) => this.velocity = velocity;
	}
}
using UnityEngine;

namespace SoulsLike
{
	public struct AnimationPlayEvent : IGameEvent
	{
		public readonly Vector3 velocity;

		public AnimationPlayEvent(Vector3 velocity) => this.velocity = velocity;
	}
}
using UnityEngine;

namespace SoulsLike
{
	public struct EnemyAnimationPlay : IGameEvent
	{
		public readonly int enemyID;
		public readonly Vector3 velocity;

		public EnemyAnimationPlay(int enemyID, Vector3 velocity)
		{
			this.enemyID = enemyID;
			this.velocity = velocity;
		}
	}
}
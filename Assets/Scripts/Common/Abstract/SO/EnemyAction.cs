using UnityEngine;

namespace SoulsLike
{
	public abstract class EnemyAction : ScriptableObject
	{
		[SerializeField] private string _actionAnimation = default;

		public string ActionAnimation => _actionAnimation;
	}
}
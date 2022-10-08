using SoulsLike.Extentions;
using UnityEngine;

namespace SoulsLike
{
	[RequireComponent(typeof(Rigidbody))]
	public class EnemyLocomotion : MonoBehaviour, IEventListener, IEventSender
	{
		private Transform _myTransform = default;
		private Rigidbody _rigidbody = default;

		private int _enemyID = default;

		private void OnEnable() => this.AddListener<EnemyAnimationPlay>(OnAnimationPlay);

		private void OnDisable() => this.RemoveListener<EnemyAnimationPlay>(OnAnimationPlay);

		public void Init()
		{
			_rigidbody = GetComponent<Rigidbody>();
			_myTransform = transform;
			_enemyID = _myTransform.GetInstanceID();
		}

		private void OnAnimationPlay(EnemyAnimationPlay eventInfo)
		{
			if(eventInfo.enemyID != _enemyID) return;
			_rigidbody.drag = 0;
			_rigidbody.velocity = eventInfo.velocity;
		}
	}
}
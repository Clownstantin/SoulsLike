using UnityEngine;

namespace SoulsLike
{
	public abstract class UpdateableComponent : MonoBehaviour, IUpdateable
	{
		private void Start()
		{
			GameManager.Register(this);
			OnStart();
		}

		protected virtual void OnStart() { }

		public abstract void OnUpdate(float delta);

		public virtual void OnFixedUpdate(float delta) { }

		public virtual void OnLateUpdate(float delta) { }

		private void OnDestroy() => GameManager.Unregister(this);
	}
}

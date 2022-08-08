using System.Collections.Generic;
using UnityEngine;

namespace SoulsLike
{
	public class GameManager : MonoBehaviour
	{
		private static List<IUpdateable> s_updateableObjects = new List<IUpdateable>();

		private GameControls _gameControls = default;

		private bool _isPaused = default;

		private void OnEnable()
		{
			_gameControls ??= new GameControls();
			_gameControls.GameActions.Pause.performed += p => OnGamePause();

			_gameControls.Enable();
		}

		private void OnDisable()
		{
			_gameControls.GameActions.Pause.performed -= p => OnGamePause();
			_gameControls.Disable();
		}

		private void Update() => RunUpdate();

		private void FixedUpdate() => RunUpdate(isFixed: true);

		private void LateUpdate() => RunUpdate(isLate: true);

		public static void Register(IUpdateable obj)
		{
			if(!s_updateableObjects.Contains(obj)) s_updateableObjects.Add(obj);
			else
			{
				Object gameObj = (Object)obj;
				Debug.Log($"{gameObj.name} is already registered");
			}
		}

		public static void Unregister(IUpdateable obj)
		{
			if(s_updateableObjects.Contains(obj)) s_updateableObjects.Remove(obj);
		}

		private void RunUpdate(bool isFixed = false, bool isLate = false)
		{
			if(!_isPaused && s_updateableObjects != null)
			{
				float delta = isFixed ? Time.fixedDeltaTime : Time.deltaTime;

				for(int i = s_updateableObjects.Count - 1; i >= 0; i--)
				{
					UpdateableComponent updateable = (UpdateableComponent)s_updateableObjects[i];

					if(isFixed) updateable.OnFixedUpdate(delta);
					else if(isLate) updateable.OnLateUpdate(delta);
					else updateable.OnUpdate(delta);
				}
			}
		}

		private void OnGamePause()
		{
			_isPaused = !_isPaused;

			if(_isPaused) Log.Send("Game is paused");
			else Log.Send("Game is resumed");
		}
	}
}

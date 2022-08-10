using System.Collections.Generic;
using SoulsLike.Extentions;
using UnityEngine;

namespace SoulsLike
{
	public class GameManager : MonoBehaviour
	{
		private static GameManager s_instance = default;

		private List<IUpdateable> s_updateableObjects = default;

		private EventManager _eventManager = default;
		private CameraHandler _cameraHandler = default;

		private GameControls _gameControls = default;
		private bool _isPaused = default;

		public static GameManager Instance => s_instance;

		public EventManager EventManager => _eventManager;
		public CameraHandler CameraHandler => _cameraHandler;

		#region Monobehavior
		private void Awake()
		{
			if(!s_instance)
			{
				s_instance = this;
				DontDestroyOnLoad(gameObject);
			}
			else Destroy(gameObject);

			s_updateableObjects = new List<IUpdateable>();

			_eventManager = GetComponentInChildren<EventManager>();
			_cameraHandler = GetComponentInChildren<CameraHandler>();
		}

		private void OnEnable()
		{
			_gameControls ??= new GameControls();
			_gameControls.GameActions.Pause.performed += _ => OnPausePress();

			_gameControls.Enable();
		}

		private void OnDisable()
		{
			_gameControls.GameActions.Pause.performed -= _ => OnPausePress();
			_gameControls.Disable();
		}

		private void Update() => RunUpdate();

		private void FixedUpdate() => RunUpdate(isFixed: true);

		private void LateUpdate() => RunUpdate(isLate: true);
		#endregion

		public void RegisterUpdatableObject(IUpdateable obj)
		{
			if(!s_updateableObjects.Contains(obj)) s_updateableObjects.Add(obj);
			else
			{
				Object gameObj = (Object)obj;
				Log.Send($"{gameObj.name} is already registered");
			}
		}

		public void UnregisterUpdatableObject(IUpdateable obj)
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

		private void OnPausePress()
		{
			_isPaused = !_isPaused;

			if(_isPaused)
			{
				this.TriggerEvent(EventID.OnGamePause);
				Log.Send("Game is paused");
			}
			else
			{
				this.TriggerEvent(EventID.OnGameResume);
				Log.Send("Game is resumed");
			}
		}
	}
}

using System.Collections.Generic;
using SoulsLike.Extentions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SoulsLike
{
	public class GameManager : MonoBehaviour
	{
		private static GameManager s_instance = default;

		private List<UpdateableComponent> _updateableObjects = default;

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

			_eventManager = new EventManager();
			_updateableObjects = new List<UpdateableComponent>();

			_cameraHandler = GetComponentInChildren<CameraHandler>();
		}

		private void OnEnable()
		{
			_gameControls ??= new GameControls();
			_gameControls.GameActions.Pause.performed += OnPausePress;

			_gameControls.Enable();
		}

		private void OnDisable()
		{
			_gameControls.GameActions.Pause.performed -= OnPausePress;
			_gameControls.Disable();
		}

		private void Update() => RunUpdate();

		private void FixedUpdate() => RunUpdate(isFixed: true);

		private void LateUpdate() => RunUpdate(isLate: true);

		private void OnDestroy() => _eventManager.Dispose();
		#endregion

		public void RegisterUpdatableObject(UpdateableComponent obj)
		{
			if(!_updateableObjects.Contains(obj)) _updateableObjects.Add(obj);
			else
			{
				var gameObj = (Object)obj;
				Log.Send($"{gameObj.name} is already registered");
			}
		}

		public void UnregisterUpdatableObject(UpdateableComponent obj)
		{
			if(_updateableObjects.Contains(obj)) _updateableObjects.Remove(obj);
		}

		private void RunUpdate(bool isFixed = false, bool isLate = false)
		{
			if(_isPaused || _updateableObjects == null) return;

			float delta = isFixed ? Time.fixedDeltaTime : Time.deltaTime;

			for(int i = _updateableObjects.Count - 1; i >= 0; i--)
			{
				if(isFixed) _updateableObjects[i].OnFixedUpdate(delta);
				else if(isLate) _updateableObjects[i].OnLateUpdate(delta);
				else _updateableObjects[i].OnUpdate(delta);
			}
		}

		private void OnPausePress(InputAction.CallbackContext _)
		{
			_isPaused = !_isPaused;

			if(_isPaused) this.TriggerEvent(new GamePause());
			else this.TriggerEvent(new GameResume());
		}
	}
}

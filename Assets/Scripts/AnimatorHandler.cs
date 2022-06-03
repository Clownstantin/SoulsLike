using UnityEngine;

namespace SoulsLike
{
	[RequireComponent(typeof(Animator))]
	public class AnimatorHandler : MonoBehaviour
	{
		[SerializeField] private float _movementClamp = 0.55f;
		[SerializeField] private float _dampTime = 0.1f;
		[SerializeField] private bool _canRotate;

		private Animator _animator;
		private int _verticalHash;
		private int _horizontalHash;

		private const string Vertical = nameof(Vertical);
		private const string Horizontal = nameof(Horizontal);

		public bool CanRotate => _canRotate;

		public void Init()
		{
			_animator = GetComponent<Animator>();
			_verticalHash = Animator.StringToHash(Vertical);
			_horizontalHash = Animator.StringToHash(Horizontal);
		}

		public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement)
		{
			float vertical = ClampedAxis(verticalMovement);
			float horizontal = ClampedAxis(horizontalMovement);
			float delta = Time.deltaTime;

			_animator.SetFloat(_verticalHash, vertical, _dampTime, delta);
			_animator.SetFloat(_horizontalHash, horizontal, _dampTime, delta);
		}

		public void StartOrStopRotation(bool rotState) => _canRotate = rotState;

		private float ClampedAxis(float movementType)
		{
			float axis;

			if(movementType > 0 && movementType < _movementClamp) axis = 0.5f;
			else if(movementType > _movementClamp) axis = 1f;
			else if(movementType < 0f && movementType > -_movementClamp) axis = -0.5f;
			else if(movementType < -_movementClamp) axis = -1f;
			else axis = 0f;

			return axis;
		}
	}
}

using UnityEngine;

namespace SoulsLike
{
	public class AnimatorHandler : MonoBehaviour
	{
		private int _verticalHash;
		private int _horizontalHash;

		public Animator animator;
		public bool canRotate;

		public const string Vertical = nameof(Vertical);
		public const string Horizontal = nameof(Horizontal);

		public void Init()
		{
			animator = GetComponent<Animator>();

			_verticalHash = Animator.StringToHash(Vertical);
			_horizontalHash = Animator.StringToHash(Horizontal);
		}

		public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement)
		{
			float vertical = ClampAxis(verticalMovement);
			float horizontal = ClampAxis(horizontalMovement);

			animator.SetFloat(_verticalHash, vertical, 0.1f, Time.deltaTime);
			animator.SetFloat(_horizontalHash, horizontal, 0.1f, Time.deltaTime);
		}

		public void CanRotate() => canRotate = true;

		public void StopRotation() => canRotate = false;

		private float ClampAxis(float axis)
		{
			if(axis > 0 && axis < 0.55f) axis = 0.5f;
			else if(axis > 0.55f) axis = 1f;
			else if(axis < 0 && axis > -0.55f) axis = -0.5f;
			else if(axis < -0.55f) axis = -1f;
			else axis = 0;

			return axis;
		}
	}
}

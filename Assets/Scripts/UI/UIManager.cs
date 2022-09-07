using UnityEngine;

namespace SoulsLike.UI
{
	public class UIManager : MonoBehaviour
	{
		[SerializeField] private UITextView _textView = default;
		[SerializeField] private HUDView _hudView = default;
		[SerializeField] private InventoryView _inventoryView = default;

		private IView[] _views = default;

		private void Awake()
		{
			_inventoryView.Init();
			_views = new IView[] { _textView, _hudView, _inventoryView };
		}

		private void OnEnable()
		{
			for(int i = 0; i < _views.Length; i++)
				_views[i].Subscribe();
		}

		private void OnDisable()
		{
			for(int i = 0; i < _views.Length; i++)
				_views[i].Unsubscribe();
		}
	}
}

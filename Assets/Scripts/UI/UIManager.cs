using UnityEngine;

namespace SoulsLike.UI
{
	public class UIManager : MonoBehaviour
	{
		[SerializeField] private UITextView _textView = default;
		[SerializeField] private HUDView _hudView = default;
		[SerializeField] private BarsView _barView = default;
		[SerializeField] private InventoryView _inventoryView = default;

		private IView[] _views = default;

		private void Awake()
		{
			_inventoryView.Init();
			_views = new IView[] { _textView, _hudView, _barView, _inventoryView };
		}

		private void OnEnable()
		{
			foreach(IView view in _views)
				view.Subscribe();
		}

		private void OnDisable()
		{
			foreach(IView view in _views)
				view.Unsubscribe();
		}
	}
}

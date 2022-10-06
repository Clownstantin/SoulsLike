using System;
using SoulsLike.Extentions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SoulsLike.UI
{
	[Serializable]
	public struct UITextView : IView
	{
		[Header("Text PopUp")]
		[SerializeField] private GameObject _interactPopUpContainer;
		[SerializeField] private GameObject _itemPopUpContainer;
		[SerializeField] private TMP_Text _interactText;
		[SerializeField] private TMP_Text _itemText;
		[SerializeField] private Image _itemImage;

		public void Subscribe()
		{
			this.AddListener<InteractTextPopUp>(OnInteractTextPopUp);
			this.AddListener<ItemTextPopUp>(OnItemTextPopUp);
		}

		public void Unsubscribe()
		{
			this.RemoveListener<InteractTextPopUp>(OnInteractTextPopUp);
			this.RemoveListener<ItemTextPopUp>(OnItemTextPopUp);
		}

		private void OnInteractTextPopUp(InteractTextPopUp eventInfo)
		{
			if(_interactPopUpContainer.activeSelf == eventInfo.isActive) return;
			_interactText.text = eventInfo.interactableText;
			_interactPopUpContainer.SetActive(eventInfo.isActive);
		}

		private void OnItemTextPopUp(ItemTextPopUp eventInfo)
		{
			if(_itemPopUpContainer.activeSelf == eventInfo.isActive) return;

			if(eventInfo.item)
			{
				_itemText.text = eventInfo.item.ItemName;
				_itemImage.sprite = eventInfo.item.ItemIcon;
			}
			_itemPopUpContainer.SetActive(eventInfo.isActive);
		}
	}
}
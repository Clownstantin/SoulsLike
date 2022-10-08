using SoulsLike.Extentions;
using UnityEngine;

namespace SoulsLike
{
    public class ItemPickUp : Interactable, IEventSender
    {
        [Header("")] [SerializeField] private WeaponItem _item;

        public override void PickUp()
        {
            this.TriggerEvent(new PickUpEvent(_item));
            this.TriggerEvent(new ItemTextPopUp(_item, true));
            Destroy(gameObject);
        }
    }
}
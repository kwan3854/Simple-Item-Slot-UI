using UnityEngine;
using UnityEngine.EventSystems;

namespace SimpleItemSlotUI.Runtime
{
    public class ItemSlot : MonoBehaviour, IDropHandler
    {
        public SlotBehaviour behaviour;
    
        public void OnDrop(PointerEventData eventData)
        {
            var droppedItem = eventData.pointerDrag;
            var draggableItem = droppedItem.GetComponent<DraggableItem>();
        
            if (draggableItem == null)
            {
                return;
            }
        
            // if this slot is not empty
            if (transform.childCount > 0)
            {
                var currentItem = transform.GetChild(0);
            
                switch (behaviour)
                {
                    case SlotBehaviour.Reject:
                        return;
                    case SlotBehaviour.Swap:
                        SwapItems(currentItem, draggableItem);
                        break;
                }
            }
            else
            {
                draggableItem.SnapTo = transform;
            }
        }

        private void SwapItems(Transform currentItem, DraggableItem draggableItem)
        {
            currentItem.SetParent(draggableItem.SnapTo);
            draggableItem.SnapTo = transform;
        }

        public enum SlotBehaviour
        {
            Reject = 0,
            Swap = 1,
        }
    }
}

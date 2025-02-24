using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SimpleItemSlotUI.Runtime
{
    [RequireComponent(typeof(Image))]
    public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        internal Transform SnapTo { get; set; }
    
        // temporal parent to move the object to when dragging
        [Header("Parent to move the object to when dragging (Can be None)")]
        [Tooltip("If not set, the object will be moved to the root of the scene")]
        [SerializeField] private Transform onDragParent;
    
        private readonly List<MaskableGraphic> _graphics = new();
    
        public void OnBeginDrag(PointerEventData eventData)
        {
            SnapTo = transform.parent;
            transform.SetParent(onDragParent);
        
            // disable raycast target to allow raycast to other objects
            _graphics.ForEach(graphic => graphic.raycastTarget = false);
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            transform.SetParent(SnapTo);
            _graphics.ForEach(graphic => graphic.raycastTarget = true);
        }

        private void Awake()
        {
            if (onDragParent == null)
            {
                // set parent to root canvas if not set
                onDragParent = transform.root;
            }
        
            _graphics.AddRange(GetComponentsInChildren<MaskableGraphic>());
        
            Debug.Assert(_graphics.Count > 0, "Image component not found in DraggableItem");
        }
    }
}

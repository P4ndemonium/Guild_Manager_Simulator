using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler,
    IPointerEnterHandler, IPointerExitHandler
{
    public ItemSaveData data;
    private Vector3 originalPosition;
    private Transform originalParent;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Only show tooltip if we aren't currently dragging the item
        if (ItemTooltip.Instance != null && !canvasGroup.blocksRaycasts == false)
        {
            ItemTooltip.Instance.Show(data);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ItemTooltip.Instance != null)
        {
            ItemTooltip.Instance.Hide();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ItemTooltip.Instance?.Hide();

        originalPosition = rectTransform.position;
        originalParent = transform.parent;

        // --- ADD THIS PART ---
        // Find the container we are leaving
        InventoryContainer oldContainer = originalParent.GetComponent<InventoryContainer>();
        if (oldContainer != null)
        {
            oldContainer.RemoveItem(this); // This clears the data from the Unit
        }
        // ----------------------

        transform.SetParent(transform.root);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        GameObject dropTarget = eventData.pointerEnter;
        InventoryContainer container = dropTarget?.GetComponentInParent<InventoryContainer>();

        if (container != null && !IsOverlapping(container))
        {
            // 1. Physically move it in Hierarchy
            transform.SetParent(container.transform);

            // 2. TELL THE CONTAINER TO RUN ITS LOGIC (This is the part that adds the data!)
            container.AddItem(this);
        }
        else
        {
            SnapBack();
        }
    }

    private bool IsOverlapping(InventoryContainer targetContainer)
    {
        foreach (Transform child in targetContainer.transform)
        {
            if (child == transform) continue;

            RectTransform otherRect = child.GetComponent<RectTransform>();
            if (otherRect != null && Intersects(rectTransform, otherRect))
            {
                return true; // Overlap detected
            }
        }
        return false;
    }

    // Helper to check if two UI rectangles touch
    private bool Intersects(RectTransform r1, RectTransform r2)
    {
        Rect rect1 = new Rect(r1.position.x, r1.position.y, r1.rect.width * r1.lossyScale.x, r1.rect.height * r1.lossyScale.y);
        Rect rect2 = new Rect(r2.position.x, r2.position.y, r2.rect.width * r2.lossyScale.x, r2.rect.height * r2.lossyScale.y);
        return rect1.Overlaps(rect2);
    }

    public void SnapBack()
    {
        transform.SetParent(originalParent);
        rectTransform.position = originalPosition;

        // Add it back to the list since the move failed
        InventoryContainer oldContainer = originalParent.GetComponent<InventoryContainer>();
        if (oldContainer != null)
        {
            oldContainer.AddItem(this);
        }
    }
}

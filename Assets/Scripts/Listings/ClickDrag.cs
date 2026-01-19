using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickDrag : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Canvas canvas;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        // Find the root canvas to calculate scale correctly
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Moves the panel to the front of the screen when you start dragging
        rectTransform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Moves the panel based on the mouse/touch delta
        // We divide by canvas scale factor so the speed matches the mouse movement
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
}

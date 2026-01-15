using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverReveal : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image image;
    private bool isHovered = false;
    [SerializeField] private Adventurer adventurer;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        // Smoothly fade the alpha based on hover state
        float targetAlpha = isHovered ? 0.5f : 0f;
        Color c = image.color;
        c.a = targetAlpha;
        image.color = c;

        if (adventurer.isHired)
        {
            c.a = 1;
            image.color = c;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }
}

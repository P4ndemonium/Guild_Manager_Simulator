using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverReveal : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image image;
    private bool isHovered = false;
    [SerializeField] private Unit adventurer;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        float targetAlpha = isHovered ? 0.5f : 0f;
        Color c = image.color;

        if (!adventurer.IsHired)
        {
            c.a = targetAlpha;
            image.color = c;
        }
        else 
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

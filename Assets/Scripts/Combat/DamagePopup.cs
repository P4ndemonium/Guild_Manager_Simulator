using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamagePopup : MonoBehaviour
{
    public TextMeshProUGUI textMesh;

    public void Setup(float damageAmount)
    {
        textMesh.SetText(damageAmount.ToString());

        // 1. Initial Scale "Pop" (Start at 0 and punch up to 1.2x size)
        transform.localScale = Vector3.zero;
        transform.DOScale(1.2f, 0.2f).SetEase(Ease.OutBack);

        // 2. Move Up (Float 50 units up over 1 second)
        // SetRelative(true) makes it move from its current position
        transform.DOMoveY(transform.position.y + 50f, 1f).SetEase(Ease.OutQuad);

        // 3. Fade Out and Destroy
        // Fades alpha to 0, then calls a function to destroy the object
        textMesh.DOFade(0, 0.5f).SetDelay(0.5f).OnComplete(() => {
            Destroy(gameObject);
        });
    }
}

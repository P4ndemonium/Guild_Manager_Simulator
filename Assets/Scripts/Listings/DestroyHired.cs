using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyHired : MonoBehaviour
{
    public Transform parentTransform;

    public void DestroyHiredChildren()
    {
        if (parentTransform == null)
        {
            Debug.LogError("Please assign a Parent Transform in the inspector!");
            return;
        }

        // We loop backwards because we are destroying objects as we go
        for (int i = parentTransform.childCount - 1; i >= 0; i--)
        {
            Transform child = parentTransform.GetChild(i);

            // Look for the component that has the isHired boolean
            if (child.TryGetComponent<Unit>(out Unit unit))
            {
                if (unit.IsHired)
                {
                    // Destroy the GameObject associated with the script
                    Destroy(child.gameObject);
                }
            }
        }
    }
}

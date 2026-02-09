using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConfirmationMemory : MonoBehaviour
{
    public static ConfirmationMemory Instance;
    public GameObject questConfirmationPanel;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    // This is the variable that "remembers" the object
    private GameObject _rememberedObject;

    // Call this to tell the script which object to remember
    public void SetObjectToRemember(GameObject target)
    {
        _rememberedObject = target;
        Debug.Log("Remembered: " + target.name);
    }

    // This is the function you link to your UI Button
    public void DestroyRememberedObject()
    {
        if (_rememberedObject != null)
        {
            // Check if the object actually has a parent
            if (_rememberedObject.transform.parent != null)
            {
                GameObject parentObject = _rememberedObject.transform.parent.gameObject;

                Debug.Log("Destroying Parent: " + parentObject.name);
                Destroy(parentObject);
            }
            else
            {
                // Fallback: If no parent exists, destroy the object itself
                Debug.LogWarning(_rememberedObject.name + " has no parent! Destroying itself instead.");
                Destroy(_rememberedObject);
            }

            // Clear the memory
            _rememberedObject = null;
        }
        else
        {
            Debug.LogWarning("Memory is empty! Nothing to destroy.");
        }
    }
}

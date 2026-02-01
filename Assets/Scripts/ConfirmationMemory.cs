using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmationMemory : MonoBehaviour
{
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
            Debug.Log("Destroying: " + _rememberedObject.name);
            Destroy(_rememberedObject);

            // Clear the memory so we don't try to destroy a null object twice
            _rememberedObject = null;
        }
        else
        {
            Debug.LogWarning("Memory is empty! Nothing to destroy.");
        }
    }
}

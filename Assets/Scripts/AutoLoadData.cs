using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoLoadData : MonoBehaviour
{
    [SerializeField] private SaveManager saveManager;

    // Start is called before the first frame update
    void Awake()
    {
        saveManager.OnLoadButtonPressed();
    }
}

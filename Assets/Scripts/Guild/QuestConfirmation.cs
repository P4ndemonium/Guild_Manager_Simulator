using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestConfirmation : MonoBehaviour
{
    public static QuestConfirmation Instance;
    public GameObject confirmation;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }
}

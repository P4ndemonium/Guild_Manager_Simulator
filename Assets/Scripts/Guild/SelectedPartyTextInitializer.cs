using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedPartyTextInitializer : MonoBehaviour
{
    private void Start()
    {
        QuestManager.Instance.selectedPartyNumText = GetComponent<TMPro.TextMeshProUGUI>();
    }
}

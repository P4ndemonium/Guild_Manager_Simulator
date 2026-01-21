using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance;

    public int currentGold;

    // Start is called before the first frame update
    void Start()
    {
        currentGold = 67;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

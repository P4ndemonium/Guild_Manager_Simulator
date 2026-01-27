using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance;

    public int gold;

    // Start is called before the first frame update
    void Start()
    {
        gold = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

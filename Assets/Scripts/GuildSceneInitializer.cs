using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuildSceneInitializer : MonoBehaviour
{
    [SerializeField] private GameObject myCanvas;

    void Start()
    {
        GameSceneManager.Instance.RegisterGuildCanvas(myCanvas);
    }
}

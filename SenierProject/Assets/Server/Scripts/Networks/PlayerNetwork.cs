using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetwork : MonoBehaviour {

    public static PlayerNetwork instance;
    public string playerName { get; private set; }

    private void Awake()
    {
        instance = this;

        playerName = "Distul#" + Random.Range(1000, 9999);
    }
}

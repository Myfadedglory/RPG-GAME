using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManger : MonoBehaviour
{
    public static PlayerManger instance;
    public Player player;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else
            instance = this;
    }
}

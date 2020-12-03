using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public Transform defaultTowerBase;
    public Transform defaultTowerTop;

    public Transform magicTowerBase;
    public Transform magicTowerTop;

    public Transform sniperTowerBase;
    public Transform sniperTowerTop;

    public Transform machineGunTowerBase;
    public Transform machineGunTowerTop;

    private static GameAssets instance;

    public static GameAssets GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }
}

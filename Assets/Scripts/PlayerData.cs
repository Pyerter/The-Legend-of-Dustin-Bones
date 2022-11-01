using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public bool[] unlockedPuns;

    public PlayerData(bool[] unlockedPuns)
    {
        this.unlockedPuns = unlockedPuns;
        /*
        this.unlockedPuns = new bool[unlockedPuns.Length];
        for (int i = 0; i < unlockedPuns.Length; i++)
        {
            this.unlockedPuns[i] = unlockedPuns[i];
        }*/
    }
}

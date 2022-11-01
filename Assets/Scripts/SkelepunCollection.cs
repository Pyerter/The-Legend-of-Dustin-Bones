using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName ="SkelepunCollection", menuName = "ScriptableObjects/SkelepunCollection")]
public class SkelepunCollection : ScriptableObject
{
    [System.Serializable]
    public struct Pair
    {
        public string value;
        public bool unlocked;
    }
    public List<Pair> puns;

    public bool[] GetUnlockedPunsList()
    {
        bool[] unlocks = new bool[puns.Count];
        for (int i = 0; i < puns.Count; i++)
        {
            unlocks[i] = puns[i].unlocked;
        }
        return unlocks;
    }

    public void SetUnlockedPunsList(bool[] unlocks)
    {
        int i = 0;
        while (i < puns.Count && i < unlocks.Length)
        {
            Pair p = puns[i];
            p.unlocked = unlocks[i];
            puns[i] = p;
        }
        if (i != unlocks.Length)
        {
            Debug.LogError("Error - trying to set " + unlocks.Length + " punlocks when only " + puns.Count + " are present.");
        }
    }

    public string GetPun(int index)
    {
        return puns[index].value;
    }

    public bool GetPunlocked(int index)
    {
        return puns[index].unlocked;
    }

    public void SetPunlocked(int index, bool unlocked)
    {
        Pair p = puns[index];
        p.unlocked = unlocked;
        puns[index] = p;
    }
}

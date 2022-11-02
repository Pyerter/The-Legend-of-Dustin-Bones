using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { if (instance == null) instance = FindObjectOfType<GameManager>(); return instance; } }

    private bool paused = false;
    public bool Paused
    {
        get { return paused; }
        set
        {
            if (value != paused)
            {
                paused = value;
                OnPause?.Invoke(this, paused);
            }
        }
    }

    public delegate void BoolGameManagerEvent(GameManager sender, bool b);
    public event BoolGameManagerEvent OnPause;

    [SerializeField]
    public SkelepunCollection skelepunCollection;

    private PlayerData playerData;
    public PlayerData PData { get { if (playerData == null) playerData = SaveSystem.LoadOrInitPlayerData(skelepunCollection.GetUnlockedPunsList()); return playerData; } private set { playerData = value; } }

    [SerializeField]
    public TMPro.TextMeshPro punPrefab;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        if (!Load())
        {
            Save();
        }
    }

    private void Start()
    {
        Paused = true;
    }

    public bool TogglePause()
    {
        this.Paused = !this.Paused;
        return Paused;
    }

    private void Update()
    {
        
    }

    public void Save()
    {
        SaveSystem.SavePlayer(PData);
    }

    public bool Load()
    {
        PlayerData data = SaveSystem.LoadOrInitPlayerData(skelepunCollection.GetUnlockedPunsList());
        if (data != null)
        {
            this.PData = data;
            skelepunCollection.SetUnlockedPunsList(data.unlockedPuns);
            return true;
        }
        return false;
    }

    public string GeneratePun(Vector2 location)
    {
        int selection = Random.Range(0, skelepunCollection.puns.Count);
        SkelepunCollection.Pair p = skelepunCollection.puns[selection];
        if (!p.unlocked)
        {
            p.unlocked = true;
            skelepunCollection.puns[selection] = p;
            PData.ReadSkelepunCollection(skelepunCollection);
            Save();
        }

        TextMeshPro tmp = Instantiate(punPrefab, location, Quaternion.identity);
        tmp.text = p.value;

        return p.value;
    }

    public void ResetPunCollection()
    {
        skelepunCollection.SetUnlockedPunsList(new bool[skelepunCollection.puns.Count]);
        PData.ReadSkelepunCollection(skelepunCollection);
        Save();
    }
}

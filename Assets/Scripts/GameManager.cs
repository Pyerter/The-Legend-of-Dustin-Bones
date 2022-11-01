using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField]
    public SkelepunCollection skelepunCollection;

    private PlayerData playerData;
    public PlayerData PData { get { if (playerData == null) playerData = SaveSystem.LoadOrInitPlayerData(skelepunCollection.GetUnlockedPunsList()); return playerData; } private set { } }

    public delegate void BoolGameManagerEvent(GameManager sender, bool b);
    public event BoolGameManagerEvent OnPause;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        PlayerData data = SaveSystem.LoadPlayer();
        if (data != null)
        {
            this.PData = data;
            skelepunCollection.SetUnlockedPunsList(PData.unlockedPuns);
        }
    }

    public bool TogglePause()
    {
        this.Paused = !this.Paused;
        return Paused;
    }

    private void Update()
    {
        if (Time.time > 10f)
        {
            skelepunCollection.SetPunlocked(1, true);
            SaveSystem.SavePlayer(PData);
        }
    }
}

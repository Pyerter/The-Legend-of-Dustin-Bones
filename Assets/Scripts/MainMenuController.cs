using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] public GameObject mainMenuWindow;
    [SerializeField] public SkelepunCollectionMenu skelepunCollectionWindow;

    private void Awake()
    {
        bool mainMenu = mainMenuWindow.activeSelf;
        skelepunCollectionWindow.gameObject.SetActive(!mainMenu);
    }

    public void CorrectActive()
    {
        bool mainMenu = mainMenuWindow.activeSelf;
        skelepunCollectionWindow.gameObject.SetActive(!mainMenu);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ToggleSkelepunCollection()
    {
        bool activated = !skelepunCollectionWindow.gameObject.activeSelf;
        skelepunCollectionWindow.gameObject.SetActive(activated);
        mainMenuWindow.SetActive(!activated);
    }

    public void ResetSkelepunCollection()
    {
        GameManager.Instance.ResetPunCollection();
        skelepunCollectionWindow.ReloadPane();
    }
}

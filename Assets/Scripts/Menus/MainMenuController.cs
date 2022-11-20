using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] public EventSystem eventSystem;
    [SerializeField] public GameObject mainMenuWindow;
    [SerializeField] public SkelepunCollectionMenu skelepunCollectionWindow;

    [SerializeField] public GameObject menuStartSelection;
    [SerializeField] public GameObject skelepunStartSelection;


    private void Awake()
    {
        if (eventSystem == null)
        {
            eventSystem = FindObjectOfType<EventSystem>();
        }

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

        if (!activated)
        {
            eventSystem.SetSelectedGameObject(menuStartSelection);
        } else
        {
            eventSystem.SetSelectedGameObject(skelepunStartSelection);
        }
    }

    public void ResetSkelepunCollection()
    {
        GameManager.Instance.ResetPunCollection();
        skelepunCollectionWindow.ReloadPane();
    }
}

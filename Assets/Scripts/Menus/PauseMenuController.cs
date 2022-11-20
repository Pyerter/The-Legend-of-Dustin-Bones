using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] public EventSystem eventSystem;
    [SerializeField] public GameObject pauseItems;
    [SerializeField] public GameObject inventoryItems;

    [SerializeField] public GameObject pauseMenuFirstSelection;
    [SerializeField] public GameObject inventoryFirstSelection;

    private void Awake()
    {
        if (eventSystem == null)
        {
            eventSystem = FindObjectOfType<EventSystem>();
        }

        GameManager.Instance.OnPause += (s, b) =>
        {
            SetItemsActive(b);
        };
        SetItemsActive(GameManager.Instance.Paused);
    }

    public void SetItemsActive(bool paused)
    {
        if (!paused)
        {
            pauseItems.SetActive(false);
            inventoryItems.SetActive(false);
        } else
        {
            if (GameManager.Instance.PauseMenuToggled)
            {
                inventoryItems.SetActive(false);
                pauseItems.SetActive(true);
                eventSystem.SetSelectedGameObject(pauseMenuFirstSelection);
            } else
            {
                pauseItems.SetActive(false);
                inventoryItems.SetActive(true);
                eventSystem.SetSelectedGameObject(inventoryFirstSelection);
            }
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

}

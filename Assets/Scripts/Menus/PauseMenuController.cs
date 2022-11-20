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

    [SerializeField] public InventoryManager inventoryManager;

    private void Awake()
    {
        if (eventSystem == null)
        {
            eventSystem = FindObjectOfType<EventSystem>();
        }
        if (inventoryManager == null)
        {
            inventoryManager = GetComponent<InventoryManager>();
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
            UpdateInventory();
        } else
        {
            if (GameManager.Instance.PauseMenuToggled)
            {
                inventoryItems.SetActive(false);
                UpdateInventory();
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

    public void UpdateInventory()
    {
        inventoryManager.ApplySkillSelections();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

}

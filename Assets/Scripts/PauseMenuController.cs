using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] public EventSystem eventSystem;
    [SerializeField] public GameObject pauseItems;

    [SerializeField] public GameObject pauseMenuFirstSelection;

    private void Awake()
    {
        if (eventSystem == null)
        {
            eventSystem = FindObjectOfType<EventSystem>();
        }

        GameManager.Instance.OnPause += (s, b) =>
        {
            pauseItems.gameObject.SetActive(b);
            if (b)
            {
                eventSystem.SetSelectedGameObject(pauseMenuFirstSelection);
            }
        };
        pauseItems.gameObject.SetActive(GameManager.Instance.Paused);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

}

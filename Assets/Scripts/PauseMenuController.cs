using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] public GameObject pauseItems;

    private void Awake()
    {
        GameManager.Instance.OnPause += (s, b) =>
        {
            pauseItems.gameObject.SetActive(b);
        };
        pauseItems.gameObject.SetActive(GameManager.Instance.Paused);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    private void Awake()
    {
        GameManager.Instance.OnPause += (s, b) =>
        {
            gameObject.SetActive(b);
        };

        gameObject.SetActive(GameManager.Instance.Paused);
    }

}

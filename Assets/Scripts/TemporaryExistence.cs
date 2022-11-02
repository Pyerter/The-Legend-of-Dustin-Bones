using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryExistence : MonoBehaviour
{
    [SerializeField]
    protected float timeDone = 0f;
    public float TimeDone { get { return timeDone; } set { timeDone = value; } }

    [SerializeField]
    protected float duration = 1f;
    public float Duration { get { return duration; } private set { duration = value; } }

    private void OnEnable()
    {
        timeDone = Time.fixedTime + Duration;
        OnAppear();
    }

    private void OnDisable()
    {
        OnDisappear();
    }

    private void FixedUpdate()
    {
        if (Time.fixedTime > timeDone)
        {
            gameObject.SetActive(false);
        }
        OnUpdate();
    }

    public virtual void OnAppear()
    {

    }

    public virtual void OnDisappear()
    {

    }

    public virtual void OnUpdate()
    {

    }
}

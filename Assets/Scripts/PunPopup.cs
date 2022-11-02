using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PunPopup : TemporaryExistence
{
    [SerializeField]
    private TextMeshPro textMesh;

    [SerializeField]
    private float moveSpeed = 2f;
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }

    [SerializeField] private float shrinkSpeed = 1f;
    [SerializeField] private float growSpeed = 1f;

    private void Awake()
    {
        if (textMesh == null)
            textMesh = GetComponent<TextMeshPro>();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        float percTimeLeft = GetPercentTimeLeft();

        Vector3 position = transform.position;
        position += new Vector3(0, moveSpeed * percTimeLeft);
        transform.position = position;

        if (percTimeLeft > 0.5f)
        {
            Vector3 scale = transform.localScale;
            scale += Vector3.one * growSpeed * Time.fixedDeltaTime;
            transform.localScale = scale;
        } 
        else
        {
            Vector3 scale = transform.localScale;
            scale -= Vector3.one * shrinkSpeed * Time.fixedDeltaTime;
            transform.localScale = scale;
        }

        Color col = textMesh.color;
        col.a = percTimeLeft;
        textMesh.color = col;
    }

    public override void OnDisappear()
    {
        Destroy(gameObject);
    }
}

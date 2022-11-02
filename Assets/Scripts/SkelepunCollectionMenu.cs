using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkelepunCollectionMenu : MonoBehaviour
{
    [SerializeField]
    ScrollRect scrollView;
    [SerializeField]
    TextMeshProUGUI contentPrefab;
    [SerializeField]
    TextMeshProUGUI unlockedCounterText;

    private void Awake()
    {
        GameManager.Instance.Load();
        if (scrollView == null)
            scrollView = GetComponentInChildren<ScrollRect>();
    }

    private void Start()
    {
        ReloadPane();
    }

    public void ReloadPane()
    {
        int children = scrollView.content.childCount;
        for (int i = children - 1; i >= 0; i--)
        {
            Destroy(scrollView.content.GetChild(i).gameObject);
        }
        List<SkelepunCollection.Pair> puns = GameManager.Instance.skelepunCollection.puns;
        int unlocked = 0;
        for (int i = 0; i < puns.Count; i++)
        {
            SkelepunCollection.Pair p = puns[i];
            TextMeshProUGUI contentItem = Instantiate(contentPrefab);
            contentItem.transform.SetParent(scrollView.content);
            contentItem.text = "" + (i + 1) + ". ";
            if (p.unlocked)
            {
                contentItem.text += p.value;
                unlocked++;
            }
            else
            {
                contentItem.text += "Not unlocked yet";
                Color col = contentItem.color;
                col = Color.gray;
                contentItem.color = col;
            }
        }
        VerticalLayoutGroup group = scrollView.content.GetComponent<VerticalLayoutGroup>();
        group.CalculateLayoutInputHorizontal();
        group.CalculateLayoutInputVertical();

        unlockedCounterText.text = "Unlocked: " + unlocked + "/" + puns.Count;
    }
}

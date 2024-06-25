using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIHighlightLayout : MonoBehaviour
{
    public bool isSelected = false;
    [Space]
    public TextMeshProUGUI[] txtViews;
    [Space]
    public GameObject objBase;
    public GameObject objSelected;

    [HideInInspector] public int rowCount = -1;
    [HideInInspector] public int colCount = -1;

    public void SetText(string text)
    {
        foreach (TextMeshProUGUI txt in txtViews) txt.text = text;
    }

    private void LateUpdate()
    {
        if (isSelected)
        {
            objBase.SetActive(false);
            objSelected.SetActive(true);
        }
        else
        {
            objBase.SetActive(true);
            objSelected.SetActive(false);
        }
    }
}

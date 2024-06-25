using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHighlightMain : MonoBehaviour
{
    public bool isSelected = false;
    [Space]
    public GameObject objBase;
    public GameObject objSelected;

    [HideInInspector] public int rowCount = -1;
    [HideInInspector] public int colCount = -1;

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControls : MonoBehaviour
{
    public bool isSelected = false;
    [Space]
    public RectTransform selectArea;
    [Space]
    public float sizeHor = 72;
    public float sizeVer = 22;

    private SelectionArea areaData = new SelectionArea();

    private void LateUpdate()
    {
        if (isSelected)
        {
            selectArea.gameObject.SetActive(true);
            selectArea.sizeDelta = new Vector2(sizeHor, sizeVer);
        }
        else
        {
            selectArea.gameObject.SetActive(false);
        }
    }

    public void InputSelectionData(SelectionArea data)
    {
        areaData = data;
    }
}

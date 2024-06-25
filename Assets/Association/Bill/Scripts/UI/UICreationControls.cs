using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICreationControls : MonoBehaviour
{
    public RectTransform parHor;
    public GameObject objHor;
    public RectTransform parVer;
    public GameObject objVer;
    [Space]
    public RectTransform parMain;
    public GameObject objMainPar;
    public GameObject objMain;
    [Space]
    public string objNameBase = "ObjRepeat_";
    public string mainNamePar = "RepeatPar_";

    private string[] nameHor = new string[]
    {
        "A", "B", "C", "D", "E", "F", "G", "H", "I", "J",
        "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T",
        "U", "V", "W", "X", "Y", "Z"
    };
    private string[] nameVer = new string[]
    {
        "01", "02", "03", "04", "05", "06", "07", "08", "09", "10",
        "11", "12", "13", "14", "15", "16", "17", "18", "19", "20",
        "21", "22", "23", "24", "25", "26", "27", "28", "29", "30",
        "31", "32", "33", "34", "35", "36", "37", "38", "39", "40",
        "41"
    };

    private SelectionArea areaData = new SelectionArea();

    private void Start()
    {
        if (objHor.GetComponent<UIHighlightLayout>() == null) return;
        if (objVer.GetComponent<UIHighlightLayout>() == null) return;
        if (objMain.GetComponent<UIHighlightMain>() == null) return;

        areaData = new SelectionArea();

        objHor.SetActive(false);
        objVer.SetActive(false);
        objMainPar.SetActive(false);
        objMain.SetActive(false);

        for (int i = 0; i < nameHor.Length; i++)
        {
            int index = i;

            GameObject obj = Instantiate(objHor, parHor);
            obj.name = objNameBase + nameHor[i];
            obj.SetActive(true);

            UIHighlightLayout contInner = obj.GetComponent<UIHighlightLayout>();
            contInner.SetText(nameHor[i]);
            contInner.rowCount = index;

            areaData.layoutHor.Add(contInner);
        }
        for (int i = 0; i < nameVer.Length; i++)
        {
            int index = i;

            GameObject obj = Instantiate(objVer, parVer);
            obj.name = objNameBase + nameVer[i];
            obj.SetActive(true);

            UIHighlightLayout contInner = obj.GetComponent<UIHighlightLayout>();
            contInner.SetText(int.Parse(nameVer[i]).ToString());
            contInner.colCount = index;

            areaData.layoutVer.Add(contInner);
        }

        for (int i = 0; i < nameHor.Length; i++)
        {
            int index = i;

            GameObject parObj = Instantiate(objMainPar, parMain);
            parObj.name = mainNamePar + nameHor[i];
            parObj.SetActive(true);

            for (int j = 0; j < nameVer.Length; j++)
            {
                int jndex = j;

                GameObject obj = Instantiate(objMain, parObj.transform);
                obj.name = objNameBase + nameHor[i] + "-" + nameVer[j];
                obj.SetActive(true);

                UIHighlightMain contInner = obj.GetComponent<UIHighlightMain>();
                contInner.rowCount = index;
                contInner.colCount = jndex;

                areaData.areaMain.Add(contInner);
            }
        }

        GetComponent<UIControls>().InputSelectionData(areaData);
    }
}

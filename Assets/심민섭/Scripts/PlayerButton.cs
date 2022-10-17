using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 버튼이 눌리면 Select Match화면이 활성화 된다.
public class PlayerButton : MonoBehaviour
{
    private GameObject selectMatch;

    private void Awake()
    {
        //selectMatch = GameObject.FindGameObjectWithTag("SelectMatch");
        selectMatch = gameObject.transform.parent.parent.GetChild(4).gameObject;
    }

    public void SelectMatchPopUp()
    {
        selectMatch.SetActive(true);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��ư�� ������ Select Matchȭ���� Ȱ��ȭ �ȴ�.
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

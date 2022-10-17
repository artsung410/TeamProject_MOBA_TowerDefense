using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 뒤로 가기
public class BackButton : MonoBehaviour
{
    private GameObject selectMatch;

    void Start()
    {
        selectMatch = gameObject.transform.parent.gameObject;
    }

    public void BackSpace()
    {
        selectMatch.SetActive(false);
    }
}

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

    // 대전하기 창 뒤로가기
    public void BackSpace()
    {
        selectMatch.SetActive(false);
    }

}

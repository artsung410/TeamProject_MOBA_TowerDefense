using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCardOpen : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    // 버튼을 누르면 모든 카드가 오픈된다.

    // 모든 카드 애니메이션 실행.
    private GameObject cardObj;
    private GameObject[] cardOpenObj;

    public void AutoOpenButton()
    {
        cardObj = gameObject.transform.parent.GetChild(0).gameObject;
        DataBaseUpdater.instance.DrawAfterUpdate();
        StartCoroutine(AnimationRun());
    }

    IEnumerator AnimationRun()
    {
        for (int i = 0; i < cardObj.transform.childCount; i++)
        {
            yield return null;
            cardObj.transform.GetChild(i).GetComponent<CardUI>().AllOpenAnimation();
        }
    }
}

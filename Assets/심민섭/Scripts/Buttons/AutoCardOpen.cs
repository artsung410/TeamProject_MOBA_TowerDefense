using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCardOpen : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    // ��ư�� ������ ��� ī�尡 ���µȴ�.

    // ��� ī�� �ִϸ��̼� ����.
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

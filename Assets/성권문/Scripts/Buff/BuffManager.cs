using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

    // ###############################################
    //             NAME : ARTSUNG                      
    //             MAIL : artsung410@gmail.com         
    // ###############################################

public class BuffManager : MonoBehaviour
{
    public static event Action<int> onBuffEvent = delegate { };

    public List<BuffIcon> buffs;
    void Start()
    {
        StartCoroutine(initBuff());
    }

    IEnumerator initBuff()
    {
        yield return new WaitForSeconds(0.1f);
        int Buffcount = GameManager.Instance.currnetBuffDatas.Count;

        for (int i = 0; i < Buffcount; i++)
        {
            buffs[i].buff = GameManager.Instance.currnetBuffDatas[i];
            onBuffEvent.Invoke(i);
        }
    }

    void AddBuff(BuffData buff)
    {
        GameManager.Instance.currnetBuffDatas.Add(buff);
    }

    void UpdateBuff()
    {
        int Buffcount = GameManager.Instance.currnetBuffDatas.Count;
        onBuffEvent.Invoke(Buffcount - 1);
    }
}

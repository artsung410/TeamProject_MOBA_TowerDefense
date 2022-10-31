using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

    // ###############################################
    //             NAME : ARTSUNG                      
    //             MAIL : artsung410@gmail.com         
    // ###############################################

public class BuffManager : MonoBehaviour
{
    public List<BuffData> currentBuffDatas = new List<BuffData>(); // 각 월드에서 생성된 모든 버프들
    public static BuffManager Instance;
    public Dictionary<BuffData, float> buffDic = new Dictionary<BuffData, float>();


    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        initBuff();
    }

    public void initBuff()
    {
        TrojanHorse data = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>();

        int count = data.cardId.Count;

        if (count == 0)
        {
            return;
        }

        for (int item = 0; item < count; item++)
        {
            if (data.cardId[item] == (int)Tower.BuffTower)
            {
                int buffCount = data.cardItems[item].buffDatas.Count;

                if (buffCount == 0)
                {
                    return;
                }

                for (int buff = 0; buff < buffCount; buff++)
                {
                    currentBuffDatas.Add(data.cardItems[item].buffDatas[buff]);
                }
            }
        }

        AssemblyBuff();
    }

    public void AddBuff(BuffData buff)
    {
        currentBuffDatas.Add(buff);
        AssemblyBuff();
    }

    public void removeBuff(BuffData buff)
    {
        currentBuffDatas.Remove(buff);
    }

    public void AssemblyBuff()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
            transform.GetChild(i).GetComponent<BuffIcon>().buff = null;
            transform.GetChild(i).GetComponent<BuffIcon>().coolTime = 0f;
            transform.GetChild(i).GetComponent<BuffIcon>().elapsedTime = 0f;
            transform.GetChild(i).GetComponent<BuffIcon>().coolTimeImage.fillAmount = 0f;
        }

        for (int i = 0; i < currentBuffDatas.Count; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
            transform.GetChild(i).GetComponent<BuffIcon>().buff = currentBuffDatas[i]; // 슬롯마다 버프데이터 세팅

            if (buffDic.Count > 0)
            {
                if (buffDic.ContainsKey(currentBuffDatas[i]))
                {
                    //Debug.Log("여긴 cool");
                    transform.GetChild(i).GetComponent<BuffIcon>().elapsedTime = buffDic[currentBuffDatas[i]];
                    buffDic.Remove(currentBuffDatas[i]);
                }
            }

            transform.GetChild(i).GetComponent<BuffIcon>().coolTime = currentBuffDatas[i].Effect_Duration; // 슬롯마다 버프쿨타임 세팅
            transform.GetChild(i).GetComponent<Image>().sprite = currentBuffDatas[i].buffIcon; // 슬롯 버프이미지 적용
            Color color = transform.GetChild(i).GetComponent<Image>().color; 
            color.a = 1f;
            gameObject.transform.GetChild(i).GetComponent<Image>().color = color; // 슬롯 버프이미지 투명도 1로 적용
        }
    }
}

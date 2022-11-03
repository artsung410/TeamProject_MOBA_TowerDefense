using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Photon.Pun;

// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com         
// ###############################################
public enum Buff_Effect
{
    AtkUP = 15001,
    HpRegenUp,
    MoveSpeedUp,
    AtkSpeedUp,
    HpUp,
    CoolTimeDown,
    AtkDown,
    HpRegenDown,
    MoveSpeedDown,
    AtkSpeedDown,
    HpDown,
}

public class BuffManager : MonoBehaviourPun
{
    public static event Action<int, float, bool> towerBuffAdditionEvent = delegate { };
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
                TowerData tower = (TowerData)data.ingameDatas[item];

                int buffCount = tower.Scriptables.Count;

                if (buffCount == 0)
                {
                    return;
                }

                for (int i = 0; i < buffCount; i++)
                {
                    BuffData buff = (BuffData)tower.Scriptables[i];
                    AddBuff(buff);
                }
            }
        }

        AssemblyBuff();
    }

    // 버프 시작
    public void AddBuff(BuffData buff)
    {
        currentBuffDatas.Add(buff);

        if (buff.TargetType == Target.Tower)
        {
            towerBuffAdditionEvent.Invoke(buff.Id, buff.EffectValue, true);
        }
    }

    //public void AddBuff(BuffData buff)
    //{
    //    currentBuffDatas.Add(buff);
    //    AssemblyBuff();
    //}

    // 버프 종료
    public void removeBuff(BuffData buff)
    {
        currentBuffDatas.Remove(buff);

        if (buff.TargetType == Target.Tower)
        {
            towerBuffAdditionEvent.Invoke(buff.Id, buff.EffectValue, false);
        }
    }

    public void removeBuff_All()
    {
        for (int i = currentBuffDatas.Count - 1; i >= 0; i--)
        {
            currentBuffDatas.Remove(currentBuffDatas[i]);
        }
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
                    transform.GetChild(i).GetComponent<BuffIcon>().elapsedTime = buffDic[currentBuffDatas[i]];
                    buffDic.Remove(currentBuffDatas[i]);
                }
            }

            transform.GetChild(i).GetComponent<BuffIcon>().coolTime = currentBuffDatas[i].EffectDuration; // 슬롯마다 버프쿨타임 세팅
            transform.GetChild(i).GetComponent<Image>().sprite = currentBuffDatas[i].BuffIcon; // 슬롯 버프이미지 적용
            Color color = transform.GetChild(i).GetComponent<Image>().color; 
            color.a = 1f;
            gameObject.transform.GetChild(i).GetComponent<Image>().color = color; // 슬롯 버프이미지 투명도 1로 적용
        }
    }
}

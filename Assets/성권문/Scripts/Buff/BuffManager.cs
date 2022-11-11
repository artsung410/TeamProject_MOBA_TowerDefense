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
public enum Buff_Effect_byTower
{
    AtkUP = 1,
    HpRegenUp,
    MoveSpeedUp,
    AtkSpeedUp,
    AtkDown,
    HpRegenDown,
    MoveSpeedDown,
    AtkSpeedDown,
}

public class BuffManager : MonoBehaviourPun
{
    public static event Action<int, float, bool> towerBuffAdditionEvent = delegate { };
    public static event Action<int, float, bool> playerBuffAdditionEvent = delegate { };
    public static event Action<int, float, bool> minionBuffAdditionEvent = delegate { };
    public List<BuffData> all_DeBuffDatass = new List<BuffData>();
    public List<BuffData> currentBuffDatas = new List<BuffData>(); // 각 월드에서 생성된 모든 버프들
    public static BuffManager Instance;
    public Dictionary<BuffData, float> buffDic = new Dictionary<BuffData, float>();



    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            initBuff();
            Debug.Log("플레이어1 initBuff");
        }
        else
        {
            StartCoroutine(delayClientInitBuff());
            Debug.Log("플레이어2 initBuff");
        }
    }

    float delayTime = 0.5f;
    IEnumerator delayClientInitBuff()
    {
        yield return new WaitForSeconds(delayTime);
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
            TowerData Towerdata = (TowerData)data.ingameDatas[item];

            if (Towerdata.TowerType == Tower_Type.Buff_Tower)
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
                    AssemblyBuff();
                }
            }

            if (Towerdata.TowerType == Tower_Type.DeBuff_Tower)
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
                    
                    photonView.RPC(nameof(RPC_AddDeBuff), RpcTarget.Others, buff.Group_ID);
                }
            }
        }
    }

    // 상대방에게 디버프 시전
    [PunRPC]
    private void RPC_AddDeBuff(int id)
    {
        Debug.Log("1to2 RPC^^^^^^");
        currentBuffDatas.Add(all_DeBuffDatass[id - 5]);
        playerBuffAdditionEvent.Invoke(id, all_DeBuffDatass[id - 5].EffectValue, true);
        AssemblyBuff();
    }

    // 버프 시작
    public void AddBuff(BuffData buff)
    {
        currentBuffDatas.Add(buff);
        playerBuffAdditionEvent.Invoke(buff.Group_ID, buff.EffectValue, true);
    }

    // 버프 종료
    public void removeBuff(BuffData buff)
    {
        currentBuffDatas.Remove(buff);
        playerBuffAdditionEvent.Invoke(buff.Group_ID, buff.EffectValue, false);
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

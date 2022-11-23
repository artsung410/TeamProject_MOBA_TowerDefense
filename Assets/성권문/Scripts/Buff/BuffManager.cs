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
// 11 - 18 플레이어스킬 버프, 디버프 추가
// ###############################################
//             NAME : HongSW                      
//             MAIL : gkenfktm@gmail.com         
// ###############################################

public enum Buff_Effect
{
    Attack_Increase = 1,
    HP_Regen_Increase,
    Move_Speed_Increase,
    Attack_Speed_Increase,
    Attack_Decrese,
    HP_Regen_Decrease,
    Move_Speed_Decrese,
    Attack_Speed_Decrese,
    Stun,
    Freezing,
    Burn,
    Airborne,
    Knockback,
}


public class BuffManager : MonoBehaviourPun
{
    public static event Action<int, float, bool> towerBuffAdditionEvent = delegate { };
    public static event Action<int, float, bool> playerBuffAdditionEvent = delegate { };
    public static event Action<int, float, bool> minionBuffAdditionEvent = delegate { };

    public BuffDatabaseList buffDB;

    [HideInInspector]
    public List<BuffData> currentBuffDatas = new List<BuffData>();           
    // 각 월드에서 생성된 모든 버프들
    public static BuffManager Instance;
    public Dictionary<BuffData, float> buffDic = new Dictionary<BuffData, float>();             // 버프 쿨타임 담을 딕셔너리(버프 정렬시도 유지하도록)

    // 모든 버프데이터를 담을 딕셔너리
    public Dictionary<int, BuffBlueprint> buffDB_Dic = new Dictionary<int, BuffBlueprint>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        int buffCount = buffDB.itemList.Count;

        for (int i = 0; i < buffCount; i++)
        {
            Debug.Log(i);
            buffDB_Dic.Add(buffDB.itemList[i].ID, buffDB.itemList[i]);
        }

        Debug.Log(PhotonNetwork.LocalPlayer.ActorNumber);
    }

    // 버프 시작
    public void AddBuff(BuffData buff)
    {
        if (buff.EffectType == Effect_Type.Buff)
        {
            currentBuffDatas.Add(buff);
            playerBuffAdditionEvent.Invoke(buff.Group_ID, buff.EffectValue, true);
        }
        else
        {
            photonView.RPC(nameof(RPC_AddDeBuff), RpcTarget.Others, buff.Group_ID);
        }

        AssemblyBuff();
    }

    // 상대방에게 디버프 시전
    [PunRPC]
    private void RPC_AddDeBuff(int id)
    {
        Debug.Log("1to2 RPC^^^^^^");
        //currentBuffDatas.Add(all_DeBuffDatass[id - 5]);
        //playerBuffAdditionEvent.Invoke(id, all_DeBuffDatass[id - 5].EffectValue, true);
        AssemblyBuff();
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

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

public enum Buff_Effect_Type
{
    Damage = 1,
    Buff,
    Debuff,
}


public class BuffManager : MonoBehaviourPun
{
    public static event Action<int, float, bool> towerBuffAdditionEvent = delegate { };
    public static event Action<int, float, bool> playerBuffAdditionEvent = delegate { };
    public static event Action<int, float, bool> minionBuffAdditionEvent = delegate { };

    public BuffDatabaseList buffDB;

    [HideInInspector]
    public List<BuffBlueprint> currentBuffDatas = new List<BuffBlueprint>();
    public List<int> currentBuffIds = new List<int>();
    // 각 월드에서 생성된 모든 버프들
    public static BuffManager Instance;
    public Dictionary<BuffBlueprint, float> buffDic = new Dictionary<BuffBlueprint, float>();             // 버프 쿨타임 담을 딕셔너리(버프 정렬시도 유지하도록)

    private void Awake()
    {
        Instance = this;
    }

    // 버프 시작
    public void AddBuff(BuffBlueprint buff)
    {
        if (buff.Type == (int)Buff_Effect_Type.Buff)
        {
            currentBuffDatas.Add(buff);
            currentBuffIds.Add(buff.ID);
            playerBuffAdditionEvent.Invoke(buff.GroupID, buff.Value, true);
        }
        else if (buff.Type == (int)Buff_Effect_Type.Debuff)
        {
            photonView.RPC(nameof(RPC_AddDeBuff), RpcTarget.Others, buff.ID);
            Debug.Log("디버프 적용1");
        }
        else
        {
            currentBuffDatas.Add(buff);
            currentBuffIds.Add(buff.ID);
        }

        AssemblyBuff();
    }

    // 상대방에게 디버프 시전
    [PunRPC]
    private void RPC_AddDeBuff(int id)
    {
        if (currentBuffIds.Contains(id))
        {
            return;
        }

        currentBuffDatas.Add(CSVtest.Instance.BuffDic[id]);
        currentBuffIds.Add(id);
        playerBuffAdditionEvent.Invoke(CSVtest.Instance.BuffDic[id].GroupID, CSVtest.Instance.BuffDic[id].Value, true);
        AssemblyBuff();
        Debug.Log("디버프 적용2");
    }

    // 버프 종료
    public void removeBuff(BuffBlueprint buff)
    {
        currentBuffDatas.Remove(buff);
        currentBuffIds.Remove(buff.ID);

        // 플레이어에 적용되었던 버프 해제
        playerBuffAdditionEvent.Invoke(buff.GroupID, buff.Value, false);
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

            transform.GetChild(i).GetComponent<BuffIcon>().coolTime = currentBuffDatas[i].Duration; // 슬롯마다 버프쿨타임 세팅
            transform.GetChild(i).GetComponent<Image>().sprite = currentBuffDatas[i].Icon; // 슬롯 버프이미지 적용
        }
    }
}

using Photon.Pun;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com         
// ###############################################

public class Player : MonoBehaviourPun
{
    public static event Action<Player> PlayerMouseDownEvent = delegate { };
    public Stats playerStats;
    public Health playerHealth;
    public Sprite playerIcon;

    private float defaultRecoveryValue = 1.5f;
    private float addRecoveryValue = 0f;
    private void Awake()
    {
        BuffManager.playerBuffAdditionEvent += incrementBuffValue;
    }
    private void OnEnable()
    {
        GameManager.Instance.CurrentPlayers.Add(gameObject);
    }
    private void OnMouseDown()
    {
        if (photonView.IsMine)
        {
            return;
        }

        PlayerMouseDownEvent.Invoke(this);

    }

    float elapsedTime;
    private void Update()
    {
        if (photonView.IsMine)
        {
            return;
        }

        elapsedTime += Time.deltaTime;
        if (elapsedTime >= 1f)
        {
            playerHealth.Regenation(defaultRecoveryValue + addRecoveryValue);
            elapsedTime = 0f;
        }
    }

    // =========================== 타워 버프 적용 처리 ===========================
    public void incrementBuffValue(int id, float addValue, bool state)
    {
        if (!photonView.IsMine)
        {
            Debug.Log(gameObject.tag + "내꺼 아님");
            return;
        }

        Debug.Log(gameObject.tag + "내꺼 멎움");
        
        // 스킬용 id : 9 ~
        if (id >= 9)
        {
            ApplyDebuff(id, addValue, state);
        }
        // 타워용 id : 1 ~ 8
        else
        {
            StartCoroutine(delayApplyBuff(id, addValue, state));
        }
    }

    // 플레이어 스탯 초기화가 이루어진 뒤에 디버프적용
    IEnumerator delayApplyBuff(int id, float addValue, bool state)
    {
        yield return new WaitForSeconds(2f);
        photonView.RPC(nameof(RPC_ApplyBuff), RpcTarget.All, id, addValue, state);
    }

    // 스킬 전용 디버프
    public void ApplyDebuff(int id, float addValue, bool state)
    {

            photonView.RPC(nameof(RPC_ApplyBuff), RpcTarget.All, id, addValue, state);
        
    }

// TODO : 버프 상대편한테 적용하게 할것
// TODO : 캐릭터 setactive false되도 스크립트 유지하게함
[PunRPC]
    public void RPC_ApplyBuff(int id, float addValue, bool state)
    {
        Debug.Log($"들어온 값들\n" +
            $" id : {id}\n" +
            $"addValue : {addValue}\n" +
            $"state : {state}");

        // 플레이어 버프 적용
        if (id == (int)Buff_Effect.AtkUP || id == (int)Buff_Effect.AtkDown)
        {
            if (state)
            {
                playerStats.attackDmg += addValue;
            }
            else
            {
                playerStats.attackDmg -= addValue;
            }
        }

        else if (id == (int)Buff_Effect.HpRegenUp || id == (int)Buff_Effect.HpRegenDown)
        {
            if (state)
            {
                addRecoveryValue += addValue;
            }
            else
            {
                addRecoveryValue -= addValue;
            }
        }

        else if (id == (int)Buff_Effect.MoveSpeedUp || id == (int)Buff_Effect.MoveSpeedDown || id == (int)Buff_Effect.SlowOfSkill)
        {
            if (state)
            {
                playerStats.MoveSpeed *= (1 + addValue); // 버프적용
                Debug.Log($"현재 이동속도 : {playerStats.MoveSpeed}");
            }
            else
            {
                playerStats.MoveSpeed /= (1 + addValue);  // 원상복구
                Debug.Log($"현재 이동속도 : {playerStats.MoveSpeed}");
            }
        }

        else if (id == (int)Buff_Effect.AtkSpeedUp || id == (int)Buff_Effect.AtkSpeedDown)
        {
            if (state)
            {
                playerStats.attackSpeed *= addValue;
            }
            else
            {
                playerStats.attackSpeed /= addValue;
            }
        }

    }
}
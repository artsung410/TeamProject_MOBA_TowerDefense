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
    public static event Action<float, bool> PlayerBuffValueApplyEvent = delegate { };

    public Stats playerStats;
    public Health playerHealth;
    public Sprite playerIcon;
    public PlayerBehaviour playerBehaviour;

    private float defaultRecoveryValue = 1.5f;
    private float addAtkValue = 0f;
    private float addRecoveryValue = 0f;
    private float addMoveSpdValue = 0f;
    private float addAtkSpdValue = 0f;

    private float prevMoveSpeed;

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

    public void incrementBuffValue(int id, float addValue, bool state)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (id <= 10000)
        {
            ApplyDebuff(id, addValue, state);
        }
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
        // 플레이어 버프 적용
        if (id == (int)Buff_Group.Attack_Increase || id == (int)Buff_Group.Attack_Decrese)
        {
            if (state)
            {
                playerStats.buffAttackDamge += addValue;
            }
            else
            {
                playerStats.buffAttackDamge = 0f;
            }
        }

        else if (id == (int)Buff_Group.HP_Regen_Increase || id == (int)Buff_Group.HP_Regen_Decrease || id == (int)Buff_Group.Burn)
        {
            if (state)
            {
                addRecoveryValue += addValue;
            }
            else
            {
                addRecoveryValue = 0f;
            }
        }

        else if (id == (int)Buff_Group.Move_Speed_Increase || id == (int)Buff_Group.Move_Speed_Decrese || id == (int)Buff_Group.Freezing)
        {
            if (state)
            {
                float temp = playerStats.moveSpeed;
                temp *= (addValue);
                playerStats.buffMoveSpeed += temp;
            }
            else
            {
                playerStats.buffMoveSpeed = 0f;
            }
        }

        else if (id == (int)Buff_Group.Attack_Speed_Increase || id == (int)Buff_Group.Attack_Speed_Decrese)
        {
            if (state)
            {
                float temp = playerStats.attackSpeed;
                temp *= (addValue);
                playerStats.buffAttackSpeed += temp;
            }
            else
            {
                playerStats.buffAttackSpeed = 0f;
            }
        }

        else if (id == (int)Buff_Group.Stun)
        {
            if (state)
            {
                playerBehaviour.OnStun(state);
            }
            else
            {
                playerBehaviour.OnStun(state);
            }
        }

        playerStats.SetBuff();

    }

    private void OnDestroy()
    {
        BuffManager.playerBuffAdditionEvent -= incrementBuffValue;
    }
}
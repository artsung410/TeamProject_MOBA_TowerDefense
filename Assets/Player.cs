using Photon.Pun;
using UnityEngine;
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
        if(photonView.IsMine)
        {
            return;
        }

        PlayerMouseDownEvent.Invoke(this);
        
    }

    // =========================== 타워 버프 적용 처리 ===========================
    public void incrementBuffValue(int id, float addValue, bool state)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        photonView.RPC("RPC_ApplyBuff", RpcTarget.All, id, addValue, state);
    }

    [PunRPC]
    public void RPC_ApplyBuff(int id, float addValue, bool state)
    {
        // 플레이어 버프 적용
        if (id == (int)Buff_Effect_byTower.AtkUP)
        {
            if (state)
            {
                Debug.Log("플레이어 공격력 증가!");
                playerStats.attackDmg += addValue;
            }
            else
            {
                Debug.Log("플레이어 공격력 증가 종료!");
                playerStats.attackDmg -= addValue;
            }
        }

        else if (id == (int)Buff_Effect_byTower.HpRegenUp)
        {

        }

        else if (id == (int)Buff_Effect_byTower.MoveSpeedUp)
        {
            if (state)
            {
                Debug.Log("플레이어 이속 증가!");
                playerStats.MoveSpeed += addValue;
            }
            else
            {
                Debug.Log("플레이어 이속 증가 종료!");
                playerStats.MoveSpeed -= addValue;
            }
        }

        else if (id == (int)Buff_Effect_byTower.AtkSpeedUp)
        {
            if (state)
            {
                Debug.Log("플레이어 공속 증가!");
                playerStats.attackSpeed += addValue;
            }
            else
            {
                Debug.Log("플레이어 공속 증가 종료!");
                playerStats.attackSpeed -= addValue;
            }
        }

        // 플레이어 디버프 적용
        else if (id == (int)Buff_Effect_byTower.AtkDown)
        {
        }

        else if (id == (int)Buff_Effect_byTower.HpRegenDown)
        {
        }

        else if (id == (int)Buff_Effect_byTower.MoveSpeedDown)
        {
        }

        else if (id == (int)Buff_Effect_byTower.AtkSpeedDown)
        {
        }
    }
}
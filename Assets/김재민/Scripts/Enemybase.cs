using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Enemybase : MonoBehaviourPun
{
    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################

    // 이동속도
   protected float moveSpeed;
    // 공격사거리
    protected float attackRange;
    // 공격력
    protected float Damage;
    // 체력
    public float HP = 100f;

    public float CurrnetHP;

    //공격 쿨타임
    protected float AttackTime;

    public string EnemyTag;

    protected void OnEnable()
    {

        if(photonView)
        CurrnetHP = HP;
    }
    //protected void OnEnable()
    //{
    //    if (PhotonNetwork.IsMasterClient)
    //    {
    //        if (PhotonNetwork.LocalPlayer.ActorNumber == 1 && photonView.IsMine)
    //        {
    //            gameObject.tag = "Blue";
    //            EnemyTag = "Red";
    //        }

    //        else
    //        {
    //            gameObject.tag = "Red";
    //            EnemyTag = "Blue";
    //        }
    //    }

    //    else
    //    {
    //        if (PhotonNetwork.LocalPlayer.ActorNumber == 2 && photonView.IsMine)
    //        {
    //            gameObject.tag = "Red";
    //            EnemyTag = "Blue";
    //        }
    //        else
    //        {
    //            gameObject.tag = "Blue";
    //            EnemyTag = "Red";
    //        }
    //    }
    //}

    public void TakeDamage(float Damage)
    {
        photonView.RPC("RPC_TakeDamage", RpcTarget.All, Damage);
    }

    [PunRPC]
    public void RPC_TakeDamage(float Damage)
    {
        CurrnetHP -= Damage;
        if(CurrnetHP <= 0)
        {
        Destroy(transform.parent.gameObject);
        }
        
    }

}

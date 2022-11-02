using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public class Enemybase : MonoBehaviourPun
{
    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################
    [Header("스텟")]
    public float moveSpeed;
    public float attackRange;
    public float Damage; 
    public float CurrnetHP;
    public float HP = 100f;
    // 체력

    [HideInInspector]
    public string EnemyTag;
    [HideInInspector]
    public string myTag;

    protected NavMeshAgent _navMeshAgent;
    protected Animator _animator;
    protected Transform Diepos;

    private CapsuleCollider _capsuleCollider;
    private bool Die = false;

    protected virtual void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _capsuleCollider = GetComponent<CapsuleCollider>();

    }
    protected virtual void OnEnable() // 생성
    {
        CurrnetHP = HP;
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 1 && photonView.IsMine)
            {
                gameObject.tag = "Blue";
                myTag = "Blue";
                EnemyTag = "Red";

            }

            else
            {
                gameObject.tag = "Red";
                myTag = "Red";
                EnemyTag = "Blue";
            }
        }
        else
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 2 && photonView.IsMine)
            {
                gameObject.tag = "Red";
                myTag = "Red";
                EnemyTag = "Blue";
            }

            else
            {
                gameObject.tag = "Blue";
                myTag = "Blue";
                EnemyTag = "Red";
            }
        }
    }
    public void TakeDamage(float Damage)
    {
        photonView.RPC("RPC_TakeDamage", RpcTarget.All, Damage);
    }

    [PunRPC]
    public void RPC_TakeDamage(float Damage)
    {
        if (Die == false)
        {

            CurrnetHP -= Damage;
            if (CurrnetHP <= 0)
            {
                Die = true;
                gameObject.GetComponent<EnemySatatus>().enabled = false;
                _capsuleCollider.enabled = false;
                _navMeshAgent.isStopped = true;
                _animator.SetTrigger("Die");
            }
        }

    }
    public void Death()
    {
        Destroy(transform.parent.gameObject);
    }

}

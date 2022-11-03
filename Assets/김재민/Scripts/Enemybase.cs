using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;
using System;

public class Enemybase : MonoBehaviourPun
{
    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################

    public static event Action<Enemybase, Sprite> minionMouseDownEvent = delegate { };
    // 이동속도
    public float moveSpeed;
    // 공격사거리
    [SerializeField]
    public float attackRange;
    // 공격력
    public float Damage;
    // 공격속도
    public float AttackSpeed = 1f;
    // 미니언 사진
    public Sprite minionSprite;
    //공격 쿨타임
    protected float AttackTime;

    // 체력
    public float HP = 100f;

    public float CurrnetHP;
    [HideInInspector]
    public string EnemyTag;
    [HideInInspector]
    public string myTag;

    protected NavMeshAgent _navMeshAgent;
    protected Animator _animator;
    private CapsuleCollider _capsuleCollider;
    protected Transform Diepos;
    bool isDead = false;



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
        if (isDead == false)
        {
            CurrnetHP -= Damage;
            if (CurrnetHP <= 0)
            {
                _capsuleCollider.enabled = false;
                _navMeshAgent.isStopped = true;
                gameObject.GetComponent<EnemySatatus>().enabled = false;
                _animator.SetTrigger("Die");
                isDead = true;
            }

        }

    }

    public void Death()
    {
        Destroy(transform.parent.gameObject);
    }

    private void OnMouseDown()
    {
        minionMouseDownEvent.Invoke(this, minionSprite);
    }

}

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

    WaitForSeconds Delay100 = new WaitForSeconds(1f);
    protected NavMeshAgent _navMeshAgent;
    protected Animator _animator;
    protected Transform Diepos;
    bool isDead = false;

    private CapsuleCollider _capsuleCollider;
    private Outline _outline;

    



    protected virtual void Awake()
    {
        _outline = GetComponent<Outline>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
    }

    protected virtual void OnEnable() // 생성
    {
        _outline.enabled = false;
        _outline.OutlineWidth = 8f;
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


    private void OnMouseDown()
    {
        minionMouseDownEvent.Invoke(this, minionSprite);
    }
    public void Death()
    {
        Destroy(transform.parent.gameObject);
    }
    public void DamageOverTime(float Damage, float Time)
    {
        Debug.Log($"damage : {Damage}\n Time : {Time}");
        photonView.RPC(nameof(RPC_DamageOverTime), RpcTarget.All, Damage, Time);
    }

    [PunRPC]
    public IEnumerator RPC_DamageOverTime(float Damage, float Time)
    {
        Debug.Log("courutine start");
        while (true)
        {
            if (CurrnetHP <= 0)
            {
                Debug.Log("으앙 나죽어");
                _capsuleCollider.enabled = false;
                _navMeshAgent.isStopped = true;
                gameObject.GetComponent<EnemySatatus>().enabled = false;
                _animator.SetTrigger("Die");
                isDead = true;
                yield return Delay100;
                yield break;
            }

            if (Time <= 0)
            {
                yield break;
            }
            CurrnetHP -= Damage;
            yield return Delay100;
            Time -= 1f;

            yield return null;
        }
    }
    private void OnMouseEnter()
    {
        if (photonView.IsMine) // 자기 자신이면 켜주고  색 그린
        {

            _outline.OutlineColor = Color.green ;
            _outline.enabled = true; // 켜주고
        }
        else
        {

            _outline.OutlineColor = Color.red;
            _outline.enabled = true;
        }
        

    }
    private void OnMouseExit()
    {
        _outline.enabled = false;
    }



}

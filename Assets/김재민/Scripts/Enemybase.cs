using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;
using System;

public enum EMINIONTYPE
{
    Nomal,
    Shot,
    Special,
    Netural,
}

public class Enemybase : MonoBehaviourPun
{
    public EMINIONTYPE _eminontpye;
    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################

    // 경험치관련 이벤트
    public static event Action<GameObject, float> OnMinionDieEvent = delegate { };

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

   public string lastDamageTeam;

    public float CurrnetHP;
    [HideInInspector]
    public string EnemyTag;
    [HideInInspector]
    public string myTag;

    WaitForSeconds Delay100 = new WaitForSeconds(1f);
    protected NavMeshAgent _navMeshAgent;
    protected Animator _animator;
    protected Transform Diepos;
    public bool isDead = false;

    public CapsuleCollider _capsuleCollider;
    private Outline _outline;





    protected virtual void Awake()
    {
        _eminontpye = EMINIONTYPE.Nomal;
        _outline = GetComponent<Outline>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
    }

    protected virtual void OnEnable() // 생성
    {
        if (GetComponent<BulletSpawn>() != null) // 타입구분
        {
            _eminontpye = EMINIONTYPE.Shot;
        }else if (GetComponent<OrcFSM>() != null)
        {
            _eminontpye = EMINIONTYPE.Netural;
        }
        _navMeshAgent.enabled = false;
        _navMeshAgent.enabled = true;
        if(gameObject.GetComponent<Outline>() != null)
        {
        _outline.enabled = false;
        _outline.OutlineWidth = 8f;
        }
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

    private void FixedUpdate()
    {
        if(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f && _animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.die"))
        {
            Death();
        }
    }

    public void TakeDamage(float Damage)
    {

        photonView.RPC("RPC_TakeDamage", RpcTarget.All, Damage);

    }

    float exp = 30f;
    [PunRPC]
    public void RPC_TakeDamage(float Damage)
    {
        if (isDead == false)
        {
            CurrnetHP -= Damage;
            if (CurrnetHP <= 0)
            {
                 
                _capsuleCollider.enabled = false;
                if (_navMeshAgent == true)
                {
                    _navMeshAgent.isStopped = true;

                }
                OnMinionDieEvent.Invoke(this.gameObject, exp);
                _animator.SetTrigger("Die");
                isDead = true;
                PlayerHUD.Instance.lastDamageTeam = lastDamageTeam;
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
                _capsuleCollider.enabled = false;
                if (_navMeshAgent.enabled == true)
                {
                    _navMeshAgent.isStopped = true;

                }
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
        if(_outline == null)
        {
            return;
        }

        if (photonView.IsMine) // 자기 자신이면 켜주고  색 그린
        {

            _outline.OutlineColor = Color.green;
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
        if (_outline == null)
        {
            return;
        }
        _outline.enabled = false;
    }



}

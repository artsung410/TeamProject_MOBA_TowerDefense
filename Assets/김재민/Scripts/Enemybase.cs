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

public enum EMINIONSIZE
{ 
    Small,
    Nomal,
    Big,

}


public class Enemybase : MonoBehaviourPun
{
    public EMINIONTYPE _eminontpye;
    public EMINIONSIZE _eminontpyeSize;
    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################
    // ###############################################
    //             NAME : Artsung                      
    //             MAIL : artsung410@gmail.com
    //             2022-11-25 /11:59 / MinionBlueprint 추가
    // ###############################################

    // 경험치관련 이벤트
    public static event Action<GameObject, float> OnMinionDieEvent = delegate { };

    public static event Action<Enemybase, Sprite> minionMouseDownEvent = delegate { };

    [Header("미니언 DB (디버그용, 추후에 Private로 전환예정")]
    public MinionBlueprint minionDB;

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
    public float HP;

    public string lastDamageTeam;

    public float CurrnetHP;
    [HideInInspector]
    public string EnemyTag;
    [HideInInspector]
    public string myTag;

    WaitForSeconds Delay100 = new WaitForSeconds(1f);
    protected NavMeshAgent _navMeshAgent;
    protected NavMeshObstacle _navMeshObstacle;
    protected Animator _animator;
    protected bool isDead = false;

    public CapsuleCollider _capsuleCollider;
    private Outline _outline;
    private GameObject skillParent;

    protected virtual void Awake()
    {

        _eminontpye = EMINIONTYPE.Nomal;
        _outline = GetComponent<Outline>();
        _navMeshObstacle = GetComponent<NavMeshObstacle>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        

    }

    protected virtual void OnEnable() // 생성
    {
        if (GetComponent<BulletSpawn>() != null) // 타입구분
        {
            _eminontpye = EMINIONTYPE.Shot;
        }
        else if (GetComponent<OrcFSM>() != null)
        {
            _eminontpye = EMINIONTYPE.Netural;
        }else if (GetComponent<SpecialAttack>() != null)
        {
            _eminontpye = EMINIONTYPE.Special;
        }
        _navMeshAgent.enabled = false;
        _navMeshAgent.enabled = true;
        if (gameObject.GetComponent<Outline>() != null)
        {
            _outline.enabled = false;
            _outline.OutlineWidth = 8f;
        }

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

        if(_eminontpye == EMINIONTYPE.Special)
        {
            skillParent = transform.parent.transform.parent.gameObject;
        }


    }

    private void Update()
    {
        if(_eminontpye == EMINIONTYPE.Netural)
        {
            OrcFSM orc = gameObject.GetComponent<OrcFSM>();
            orc.setNeturalMonsterHealthBar();
            orc.HealthUI.transform.position = transform.position;         
        }
        //Debug.Log($"{_animator.GetCurrentAnimatorStateInfo(0).normalizedTime}으앙80퍼되서쥬금");
        if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f && _animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.die"))
        {
            //Debug.Log("으앙80퍼되서쥬금");

            //if (photonView.IsMine)
            //{
            //}
                Death();
        }
    }

    public void SetInitData(int id)
    {
        photonView.RPC(nameof(RPC_SetInitData), RpcTarget.All, id);
    }

    [PunRPC]
    public void RPC_SetInitData(int id)
    {
        minionDB = CSVtest.Instance.MinionDic[id];

        Damage = minionDB.Attack;
        AttackSpeed = minionDB.Attack_Speed;
        _animator.SetFloat("Speed", AttackSpeed);
        attackRange = minionDB.Range;
        moveSpeed = minionDB.Move_Speed;
        HP = CurrnetHP = minionDB.Hp;
        minionSprite = minionDB.Icon_Blue;

        //Debug.Log($"[미니언] { PhotonNetwork.LocalPlayer.ActorNumber}월드 {gameObject.name} 초기데이터 세팅 완료");
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
            if (_eminontpye != EMINIONTYPE.Netural)
            {
                CurrnetHP -= Damage;
            }
            else
            {
                CurrnetHP -= Damage * (PlayerHUD.Instance.min + 1); // 1안더해주면 0분일때 데미지 안드감
                _animator.SetTrigger("TakeDamage");
               
            }
            if (CurrnetHP <= 0)
            {
                if (_eminontpye == EMINIONTYPE.Netural)
                {
                    GameManager.Instance.winner = lastDamageTeam;
                    PlayerHUD.Instance.NeutalMonsterDie = true;
                    PlayerHUD.Instance.min = 0;
                    PlayerHUD.Instance.sec = 0;

                }
                OnMinionDieEvent.Invoke(this.gameObject, exp);
                _capsuleCollider.enabled = false;
                if (_navMeshAgent != null)
                {
                    if (_navMeshAgent == null)
                    {
                        return;
                    }

                    _navMeshAgent.SetDestination(transform.position);
                    _navMeshAgent.isStopped = true;

                }
                Debug.Log("으앙다이상태임쥬금");
                _animator.SetTrigger("Die");
                isDead = true;

            }
        }
    }

    public void tagThrow(string value)
    {
        photonView.RPC(nameof(RPC_tagThrow), RpcTarget.All, value);
    }

    [PunRPC]
    public void RPC_tagThrow(string value)
    {
        if(PlayerHUD.Instance.NeutalMonsterDie == true)
        {
            return;
        }
        lastDamageTeam = value;
    }

    private void OnMouseDown()
    {
        minionMouseDownEvent.Invoke(this, minionSprite);
    }

    public void Death()
    {
        Debug.Log("으앙쥬금");
        if (_eminontpye == EMINIONTYPE.Special)
            {
            Destroy(transform.parent.gameObject); 
            Destroy(skillParent);
            return;
            }
       if(photonView.IsMine)
        {
        PhotonNetwork.Destroy(transform.parent.gameObject);

        }

        
    }


    public void DamageOverTime(float Damage, float Time)
    {

        photonView.RPC(nameof(RPC_DamageOverTime), RpcTarget.All, Damage, Time);
    }

    [PunRPC]
    public IEnumerator RPC_DamageOverTime(float Damage, float Time)
    {
        Debug.Log("courutine start");
        while (false == isDead)
        {
            if (isDead)
            {
                yield break;
            }
            
            if (CurrnetHP <= 0)
            {
                if (_eminontpye == EMINIONTYPE.Netural) // 중립몬스터이면 막타데미지를
                {
                    GameManager.Instance.winner = lastDamageTeam;
                    PlayerHUD.Instance.NeutalMonsterDie = true;
                }
                OnMinionDieEvent.Invoke(this.gameObject, exp);
                _capsuleCollider.enabled = false;
                if (_navMeshAgent.enabled == true)
                {
                    _navMeshAgent.isStopped = true;

                }
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
        if (_outline == null)
        {
            return;
        }

        if (photonView.IsMine) // 자기 자신이면 켜주고  색 그린
        {
            Cursor.SetCursor(PlayerHUD.Instance.cursorMoveAlly, Vector2.zero, CursorMode.Auto);
            _outline.OutlineColor = Color.green;
            _outline.enabled = true; // 켜주고
        }
        else
        {
            Cursor.SetCursor(PlayerHUD.Instance.cursorMoveEnemy, Vector2.zero, CursorMode.Auto);
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

    public void ForSkillAgent(Vector3 destination)
    {
        _navMeshAgent.isStopped = false;
        _navMeshAgent.SetDestination(destination);
    }

    public void AtkSpeedUp(float atkSpeedBuff)
    {
        float value = atkSpeedBuff / 100f;
        AttackSpeed = (1f + value);
        _animator.SetFloat("Speed", AttackSpeed);
        Debug.Log($"{AttackSpeed}");
    }

    public void atkSpeedReset(float atkSpeed)
    {
        _animator.SetFloat("Speed", atkSpeed);
    }
}

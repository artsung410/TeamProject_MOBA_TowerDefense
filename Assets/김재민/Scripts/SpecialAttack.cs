using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpecialAttack : Enemybase
{
    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################
    public Transform target;
    public SpecialMinionSkill _skill;
    private WaitForSeconds Delay;
    public float LifeTime = 15f;
    float elaspedTime = 0f;

    [SerializeField]
    private GameObject Effect;


    protected override void Awake()
    {
        base.Awake();
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(2).transform.GetChild(0).GetComponent<DragonAttack>().Damage = Damage;
        
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        _navMeshAgent.enabled = false;
    }
    private void Start()
    {
        
        if (gameObject.CompareTag("Blue"))
        {
            transform.GetChild(2).transform.GetChild(0).GetComponent<DragonAttack>().EnemyTag = EnemyTag;
        }
        else
        {
            transform.GetChild(2).transform.GetChild(0).GetComponent<DragonAttack>().EnemyTag = EnemyTag;
        }



    }
    // Update is called once per frame
    void Update()
    {


        if (_skill.TargetOn == true)
        {
            Attack();

        }

    }

    public void AttackboxOn() // 데미지 처리
    {

        transform.GetChild(2).gameObject.SetActive(true);
    }

    public void AttackboxOff() // 데미지 처리
    {

        transform.GetChild(2).gameObject.SetActive(false);
    }



    private void Attack()
    {     
       


        if (target == null)
        {
            target = null;
            Debug.Log("용 타켓이 없을때");
            _navMeshAgent.isStopped = false; //스탑 풀어주고
            FireBressRender(false);
            Attackanimation(false);
            _skill.TargetOn = false;
            _capsuleCollider.enabled = false;
            _navMeshAgent.enabled = false;
            CombackSon();
            return;
        }
        float vecDistance = Vector3.SqrMagnitude(target.position - transform.position);

        if(vecDistance > attackRange * attackRange && _animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.FireBreathLeftRight") && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f )
        {
            target = null;

            return;
        }
        _navMeshAgent.SetDestination(target.position);
        transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
        if (vecDistance <= attackRange * attackRange)
        {
            float attackingDistance = Vector3.Distance(target.position, transform.position);
            _navMeshAgent.isStopped = true;
            Attackanimation(true); //공격

        }
       


    }
    void Attackanimation(bool attackon)
    {
        photonView.RPC(nameof(RPC_Attackanimation), RpcTarget.All, attackon);
    }
    [PunRPC]
    void RPC_Attackanimation(bool attackon)
    {
        _animator.SetBool("Attack", attackon);
    }


    void CombackSon()
    {
        if (_skill == null)
        {
            return;
        }

        photonView.RPC(nameof(RPC_CombackSon), RpcTarget.All);
    }
    [PunRPC]
    void RPC_CombackSon()
    {
        Effect.SetActive(false);
        transform.position = new Vector3(_skill.gameObject.transform.position.x, _skill.gameObject.transform.position.y, _skill.gameObject.transform.position.z + 10);
        transform.rotation = Quaternion.Euler(0, 90, 0);
        Effect.SetActive(true);
        transform.parent.transform.parent = _skill.gameObject.transform; // 부모에 넣어주기전에


    }

    void FireBressRender(bool value)
    {
        photonView.RPC(nameof(RPC_FireBressRender), RpcTarget.All, value);
    }

    [PunRPC]
    void RPC_FireBressRender(bool value)
    {
        transform.GetChild(2).gameObject.SetActive(value);
    }








}

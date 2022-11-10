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
    public GameObject Mother;



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
        _navMeshAgent.enabled = true;

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

        if (target == null)
        {
            return;
        }


        Attack();

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
        if (photonView.IsMine)
        {

            if (target == null)
            {
                _navMeshAgent.isStopped = false; //스탑 풀어주고
                Attackanimation(false);
                CombackSon();



            }
            _navMeshAgent.SetDestination(target.position);
            transform.LookAt(new Vector3(target.position.x, 0, target.position.z));
            float vecDistance = Vector3.SqrMagnitude(target.position - transform.position);
            Debug.Log($"vecDistance :{vecDistance} , attackRange : {attackRange * attackRange}");
            if (vecDistance <= attackRange * attackRange)
            {
                _navMeshAgent.isStopped = true;
                Attackanimation(true); //공격

            }
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
        photonView.RPC(nameof(RPC_CombackSon), RpcTarget.All);
    }
    [PunRPC]
    void RPC_CombackSon()
    {
        transform.parent = Mother.transform;
    }
}

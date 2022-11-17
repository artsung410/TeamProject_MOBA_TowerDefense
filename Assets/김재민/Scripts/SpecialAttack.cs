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

    protected override void Awake()
    {
        base.Awake();  
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(2).transform.GetChild(0).GetComponent<DragonAttack>().Damage = Damage;
        target = null;
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
       if(_skill.TargetOn == true)
        {
            Attack();

        }

    }

    public void AttackboxOn() // ������ ó��
    {

        transform.GetChild(2).gameObject.SetActive(true);
    }

    public void AttackboxOff() // ������ ó��
    {

        transform.GetChild(2).gameObject.SetActive(false);
    }


   
    private void Attack()
    {

            if (target == null)
            {
                target = null;
                Debug.Log("�� Ÿ���� ������");
                _navMeshAgent.isStopped = false; //��ž Ǯ���ְ�
                transform.GetChild(2).gameObject.SetActive(false);
                Attackanimation(false);
                _skill.TargetOn = false;
            _capsuleCollider.enabled = false;
            _navMeshAgent.enabled = false;
                CombackSon();
            return;
            }

      
        _navMeshAgent.SetDestination(target.position);
            transform.LookAt(new Vector3(target.position.x, 0, target.position.z));
            float vecDistance = Vector3.SqrMagnitude(target.position - transform.position);
            if (vecDistance <= attackRange * attackRange)
            {
                _navMeshAgent.isStopped = true;
                Attackanimation(true); //����

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
        transform.position = new Vector3(_skill.gameObject.transform.position.x,_skill.gameObject.transform.position.y,_skill.gameObject.transform.position.z + 10);
        transform.rotation = Quaternion.Euler(0,90,0);
        transform.parent.transform.parent = _skill.gameObject.transform; // �θ� �־��ֱ�����

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;


public class SpecialMinionSkill : SkillHandler
{

    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################

    // �÷��̾� ��ġ�� ���ƾ��� �Ÿ��� �˾ƾ���
    float speed = 100f;
    float distance = 5f;
    string enemyTag;
    float elapsedTime;
    public bool TargetOn;


    [SerializeField]


    //public static SpecialMinionSkill Instance { get; private set; }

    private void Awake()
    {

        //Instance = this;   
    }
    private void Start()
    {

        if (_ability == null) return;
        TagProcessing(_ability);
        gameObject.tag = GetMytag(_ability);
        InvokeRepeating(nameof(nearFindObject), 0, 0.5f);


    }

    private void TagProcessing(HeroAbility ability)// ���±� �˾ƾ��� ��������
    {

        if (ability.CompareTag("Blue"))
        {
            enemyTag = "Red";
        }
        else if (ability.CompareTag("Red"))
        {
            enemyTag = "Blue";
        }
    }

    void FixedUpdate()
    {
        if (_ability == null)
        {
            return;
        }
        if (_ability.gameObject.GetComponent<Health>().isDeath == true)
        {
            PhotonNetwork.Destroy(gameObject);
        }

        if (photonView.IsMine)
        {
            SkillUpdatePosition();

        }
    }

    public override void SkillHoldingTime(float time) // ���ӽð�
    {

    }

    public override void SkillUpdatePosition() // �̰� �ʿ��� ȣȣ Ʈ�������� �ȳ־ �۵��ȉ���
    {
        this.gameObject.transform.position = _ability.gameObject.transform.position;
        gameObject.transform.Rotate(Vector3.up * Time.deltaTime * speed);
    }

    private void RunAway()
    {
        photonView.RPC(nameof(RPC_RunAway), RpcTarget.All);
    }

    [PunRPC]

    public void RPC_RunAway()
    {
        transform.GetChild(0).transform.GetChild(0).GetComponent<CapsuleCollider>().enabled = true;
        transform.GetChild(0).transform.GetChild(0).GetComponent<NavMeshAgent>().enabled = true;
        transform.DetachChildren();
    }

   private void nearFindObject()
    {
        Collider[] Enemys = Physics.OverlapSphere(transform.position, 10f);
        foreach (Collider col in Enemys)
        {
            if (col.gameObject.tag == GetMytag(_ability))
            {
                continue;
            }
            if (TargetOn == false)
            {


                if (col.gameObject.CompareTag(enemyTag))
                { // ��ų -> �������Ʈ -> 
                    if (transform.GetChild(0) != null)
                    {
                        gameObject.transform.GetChild(0).GetChild(0).GetComponent<SpecialAttack>().target = col.transform;
                        TargetOn = true;
                        RunAway();
                    }
                }
            }
        }
    }




}

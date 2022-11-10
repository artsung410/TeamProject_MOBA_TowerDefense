using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


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
        transform.GetChild(0).GetComponent<SpecialAttack>().Mother = gameObject;
        if (_ability == null) return;
        TagProcessing(_ability);
        gameObject.tag = GetMytag(_ability);
        
    
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

    public override void SkillUpdatePosition() // �̰� �ʿ��� ȣȣ 
    {
        this.gameObject.transform.position = _ability.gameObject.transform.position;
        gameObject.transform.Rotate(Vector3.up * Time.deltaTime * speed);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (enemyTag == null) return;

        if(other.CompareTag(enemyTag))
        {
            TargetOn = true;
         
            if(transform.GetChild(0) != null)
            {
                transform.GetChild(0).GetComponent<SpecialAttack>().target = other.gameObject.transform;
                
                RunAway();


            }
        }
    }

    private void RunAway()
    {
        photonView.RPC(nameof(RPC_RunAway), RpcTarget.All);
    }

    [PunRPC]

    private void RPC_RunAway()
    {
        transform.DetachChildren();
    }

    
    private void tagAlltarget()
    {
        photonView.RPC(nameof(RpcTarget), RpcTarget.All);
    }

   







}

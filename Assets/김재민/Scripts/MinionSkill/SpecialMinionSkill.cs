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

    // 플레이어 위치를 돌아야함 거리를 알아야함
    float speed = 100f;
    float distance = 5f;
    float elapsedTime;
    public bool TargetOn;
    [SerializeField]
    private GameObject dragon;

    [SerializeField]
    private GameObject Effect;




    //public static SpecialMinionSkill Instance { get; private set; }

    private void Awake()
    {

        //Instance = this;   
        
    }
    private void Start()
    {
        
        elapsedTime = 0f;
        if (_ability == null) return;
        transform.GetChild(0).transform.GetChild(0).GetComponent<Enemybase>().SetInitData((int)Data.Value_1);


        _ability.OnLock(true);
        gameObject.tag = GetMytag(_ability);
        InvokeRepeating(nameof(nearFindObject), 0, 0.5f);
        dragon = transform.GetChild(0).gameObject;

    }

    void FixedUpdate()
    {
        if (_ability == null)
        {
            return;
        }
        if (_ability.gameObject.GetComponent<Health>().isDeath == true)
        {
            if (photonView.IsMine)
            {
             PhotonNetwork.Destroy(gameObject);
            }
        }

        if (gameObject == null)
        {
            return;
        }

        if (photonView.IsMine)
        {

            SkillUpdatePosition();
            SkillHoldingTime(15f);   
        }

        if(dragon == null)
        {
            if(photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    private void LateUpdate()
    {
        
    }

    public override void SkillHoldingTime(float time) // 지속시간
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= Data.LockTime)
        {
            _ability.OnLock(false);
        }
        if(elapsedTime >= time)
        {
            PhotonNetwork.Destroy(gameObject);
        }

    }

    public override void SkillUpdatePosition() // 이거 필요함 호호 트랜스폼뷰 안넣어서 작동안瑛
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

        Effect.SetActive(false);
        transform.GetChild(0).transform.GetChild(0).GetComponent<CapsuleCollider>().enabled = true;
        transform.GetChild(0).transform.GetChild(0).GetComponent<NavMeshAgent>().enabled = true;
        transform.DetachChildren();
        Effect.SetActive(true); 

    }

   private void nearFindObject()
    {
        if (gameObject == null)
        {
            return;
        }

        Collider[] Enemys = Physics.OverlapSphere(transform.position, 15f);
        foreach (Collider col in Enemys)
        {
            if (col.gameObject.tag == GetMytag(_ability))
            {
                continue;
            }
            if (TargetOn == false)
            {


                if (col.gameObject.CompareTag(enemyTag))
                { // 스킬 -> 빈오브젝트 -> 
                    if (transform.GetChild(0) != null)
                    {
                        gameObject.transform.GetChild(0).GetChild(0).GetComponent<SpecialAttack>().target = col.transform;
                        gameObject.transform.GetChild(0).transform.position = col.transform.position;
                        gameObject.transform.GetChild(0).GetChild(0).transform.position = gameObject.transform.GetChild(0).transform.position + new Vector3(0,0, 4f);
                        TargetOn = true;
                        RunAway();
                    }
                }
            }
        }
    }








}

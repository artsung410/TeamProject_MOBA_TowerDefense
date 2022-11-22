using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Firecolumn : SkillHandler
{
    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################

    float Damage = 5f;
    private float elapsedTime;
    private float holdlingTime = 15f;
    //string enemyTag;
    private float dotTime = 5f; // 지속시간  
    SphereCollider sphereCollider;


    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();    
    }

    void Start()
    {
        if (_ability == null)
        {
            return;
        }
        //TagProcessing(_ability);

    }

    // Update is called once per frame
    void Update()
    {
        SkillHoldingTime(holdlingTime);
    }



    public override void SkillHoldingTime(float time)
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= time)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }



    public override void SkillUpdatePosition()
    {

    }

    //private void TagProcessing(HeroAbility ability)
    //{

    //    if (ability.CompareTag("Blue"))
    //    {
    //        enemyTag = "Red";
    //    }
    //    else if (ability.CompareTag("Red"))
    //    {
    //        enemyTag = "Blue";
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine)
        {

            if (other.CompareTag(enemyTag))
            {
                SkillTimeDamage(Damage, dotTime, other.gameObject); // 타겟 게임오브젝트 지속 데미지
            }
        }

    }





}

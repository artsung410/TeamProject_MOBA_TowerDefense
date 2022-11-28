using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MinionAttack : MonoBehaviourPun
{
    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################

    BoxCollider boxColider;
    EnemySatatus satatus;

    private void Awake()
    {
        satatus = GetComponent<EnemySatatus>();
        boxColider = GetComponent<BoxCollider>();
    }
    private void OnEnable()
    {
        boxColider.enabled = false;
    }


    //TODO : 데미지 연산부분 이즈마인 처리해서 한번만 들어가게끔 처리해야함
    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine)
        {
            
            EnemyTagNullCheck();
            if (other.CompareTag(satatus.EnemyTag))
            {

                if (other.gameObject.layer == 8) // 미니언 공격
                {

                    other.gameObject.GetComponent<Enemybase>().TakeDamage(satatus.Damage);

                }
                else if (other.gameObject.layer == 7) // 플레이어 공격
                {
                    other.gameObject.GetComponent<Health>().OnDamage(satatus.Damage);

                }
                else if (other.gameObject.layer == 6) // 타워
                {
                    other.gameObject.GetComponent<Turret>().TakeDamage(satatus.Damage);

                }
                else if (other.gameObject.layer == 12) // 넥서스
                {
                    other.gameObject.GetComponent<NexusHp>().TakeOnDagmage(satatus.Damage);

                }
                else if (other.gameObject.layer == 13) // 특수미니언
                {
                    other.gameObject.GetComponent<Enemybase>().TakeDamage(satatus.Damage);

                }
            }
            else if (other.gameObject.layer == 17)
            {
                other.gameObject.GetComponent<Enemybase>().TakeDamage(satatus.Damage);
                other.gameObject.GetComponent<Enemybase>().tagThrow(gameObject.tag);
            }
        }

    }

    public void EnemyTagNullCheck()
    {
        if (satatus.EnemyTag == null)
        {
            return;
        }
    }

    public void AttackboxOn()
    {
        boxColider.enabled = true;
    }

    public void AttackboxOff()
    {
        boxColider.enabled = false;
    }


}


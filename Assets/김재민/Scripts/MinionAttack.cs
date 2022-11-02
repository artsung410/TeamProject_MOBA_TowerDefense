using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionAttack : MonoBehaviour
{
    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################
    [SerializeField]
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


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(satatus.EnemyTag) == false)
        {
            return;
        }

        if(other.CompareTag(satatus.EnemyTag))
        {
            EnemyTagNullCheck();
            //Debug.Log("여기들어오는건가?");
            if(other.gameObject.layer == 8 && satatus._eminiomtype == EnemySatatus.EMINIOMTYPE.Nomal) // 미니언 공격
            {
                other.gameObject.GetComponent<Enemybase>().TakeDamage(satatus.Damage);
            }
            if (other.gameObject.layer == 7 &&  satatus._eminiomtype == EnemySatatus.EMINIOMTYPE.Nomal) // 플레이어 공격
            {
                other.gameObject.GetComponent<Health>().OnDamage(satatus.Damage);
            }
            if (other.gameObject.layer == 6) // 타워
            {
                other.gameObject.GetComponent<Turret>().TakeDamage(satatus.Damage);
            }
            if (other.gameObject.layer == 12) // 넥서스
            {
                other.gameObject.GetComponent<NexusHp>().TakeOnDagmage(satatus.Damage);
            }
            if (other.gameObject.layer == 13 && satatus._eminiomtype == EnemySatatus.EMINIOMTYPE.Nomal) // 특수미니언
            {
                other.gameObject.GetComponent<Enemybase>().TakeDamage(satatus.Damage);
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


    public void Attackboxoff()
    {
        boxColider.enabled = false;
    }
}

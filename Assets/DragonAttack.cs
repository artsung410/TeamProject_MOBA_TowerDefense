using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class DragonAttack : MonoBehaviourPun
{

    public string EnemyTag;
    public float Damage;
    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################


    private void Start()
    {
        Debug.Log($"{EnemyTag}");
    }

    private void OnTriggerEnter(Collider other) // 데미지 처리 박스트리거 이용함
    {

        if (photonView.IsMine)
        {
            if (other.CompareTag(EnemyTag))
            {

                if (other.gameObject.layer == 8 || other.gameObject.layer == 13) // 미니언 공격 원거리 미니언 공격이 안드감
                {
                    other.gameObject.GetComponent<Enemybase>().TakeDamage(Damage);
                }
                else if (other.gameObject.layer == 7) // 플레이어 공격
                {
                    other.gameObject.GetComponent<Health>().OnDamage(Damage);
                }
                else if (other.gameObject.layer == 6) // 타워
                {
                    other.gameObject.GetComponent<Turret>().TakeDamage(Damage);
                }
                else if (other.gameObject.layer == 12) // 넥서스
                {
                    other.gameObject.GetComponent<NexusHp>().TakeOnDagmage(Damage);
                }
                // 특수미니언 // 14우물 


            } else if (other.gameObject.layer == 17)
            {
                other.gameObject.GetComponent<Enemybase>().TakeDamage(Damage);
            }

        }
        
    }
}

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

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnTriggerEnter(Collider other) // 데미지 처리 박스트리거 이용함
    {
      
       
            if (other.CompareTag(EnemyTag))
            {
                if (photonView.IsMine)
                {
                    if (other.gameObject.layer == 8) // 미니언 공격
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
                    else if (other.gameObject.layer == 13) // 특수미니언
                    {
                        other.gameObject.GetComponent<Enemybase>().TakeDamage(Damage);
                    }
                }

            }

        


    }
}

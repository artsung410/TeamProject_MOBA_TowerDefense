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

    private void OnTriggerEnter(Collider other) // ������ ó�� �ڽ�Ʈ���� �̿���
    {

        if (photonView.IsMine)
        {
            if (other.CompareTag(EnemyTag))
            {

                if (other.gameObject.layer == 8 || other.gameObject.layer == 13) // �̴Ͼ� ���� ���Ÿ� �̴Ͼ� ������ �ȵ尨
                {
                    other.gameObject.GetComponent<Enemybase>().TakeDamage(Damage);
                }
                else if (other.gameObject.layer == 7) // �÷��̾� ����
                {
                    other.gameObject.GetComponent<Health>().OnDamage(Damage);
                }
                else if (other.gameObject.layer == 6) // Ÿ��
                {
                    other.gameObject.GetComponent<Turret>().TakeDamage(Damage);
                }
                else if (other.gameObject.layer == 12) // �ؼ���
                {
                    other.gameObject.GetComponent<NexusHp>().TakeOnDagmage(Damage);
                }
                // Ư���̴Ͼ� // 14�칰 


            } else if (other.gameObject.layer == 17)
            {
                other.gameObject.GetComponent<Enemybase>().TakeDamage(Damage);
            }

        }
        
    }
}

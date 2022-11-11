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

    private void OnTriggerEnter(Collider other) // ������ ó�� �ڽ�Ʈ���� �̿���
    {
      
       
            if (other.CompareTag(EnemyTag))
            {
                if (photonView.IsMine)
                {
                    if (other.gameObject.layer == 8) // �̴Ͼ� ����
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
                    else if (other.gameObject.layer == 13) // Ư���̴Ͼ�
                    {
                        other.gameObject.GetComponent<Enemybase>().TakeDamage(Damage);
                    }
                }

            }

        


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    HeroCombat combat;

    private void Awake()
    {
        combat = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroCombat>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // enemy�� ������� targeted�� null�϶� �ο��� ��
        if (other.CompareTag("Enemy"))
        {
            if (combat != null)
            {
                combat.MeleeAttack();
                Debug.Log("������");
            }
            else
            {
                Debug.Log("no ����");
                return;
            }
        }
    }
}

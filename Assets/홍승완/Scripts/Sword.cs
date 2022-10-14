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
        // enemy에 닿았지만 targeted가 null일때 널오류 남
        if (other.CompareTag("Enemy"))
        {
            if (combat != null)
            {
                combat.MeleeAttack();
                Debug.Log("접촉함");
            }
            else
            {
                Debug.Log("no 접촉");
                return;
            }
        }
    }
}

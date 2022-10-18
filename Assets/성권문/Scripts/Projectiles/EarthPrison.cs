using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com         
// ###############################################

public class EarthPrison : MonoBehaviourPun
{
    public float damage = 30;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Damage(other.gameObject.transform);
        }
    }

    void Damage(Transform enemy)
    {
        EnemyMinion e = enemy.GetComponent<EnemyMinion>();

        if (e != null)
        {
            e.TakeDamage(damage);
            Debug.Log("암석 데미지 적용");
        }
    }
}

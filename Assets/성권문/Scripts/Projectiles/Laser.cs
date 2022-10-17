using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com         
// ###############################################
public class Laser : MonoBehaviour
{
    public float damage = 20;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
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
            Debug.Log("레이저 데미지 적용");
        }
    }
}

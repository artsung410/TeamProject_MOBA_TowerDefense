using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedAttackRange : MonoBehaviour
{
    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################
    
    EnemySatatus Pistion;

    int Hp = 100;
    private void Awake()
    {
        Pistion = GetComponent<EnemySatatus>();

    }

    // Update is called once per frame
    void Update()
    {
        Collider[] Target = Physics.OverlapSphere(transform.position, 5f);

        foreach (Collider col in Target)
        {
            if (col.CompareTag("RedMinion"))
            {
                continue;
            }

            if (col.CompareTag("BlueMinion"))
            {
                Pistion._target = col.gameObject.transform;
                          }



        }


        if (Hp <= 0)
        {
            Destroy(gameObject);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BlueBullet"))
        {
            Hp -= other.GetComponent<BulletMove>().Damage;
        }
    }


}
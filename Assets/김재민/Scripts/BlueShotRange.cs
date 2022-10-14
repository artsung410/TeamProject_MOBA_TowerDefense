using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueShotRange : MonoBehaviour
{
    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################

    int Hp = 100;
    ShotEnemy Shot;
    private void Awake()
    {
      
        Shot = GetComponent<ShotEnemy>();
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] Target = Physics.OverlapSphere(transform.position, 5f);

        foreach (Collider col in Target)
        {
            if (col.CompareTag("BlueMinion"))
            {
                continue;
            }

            if (col.CompareTag("RedMinion"))
            {

                Shot._target = col.gameObject.transform;
             
            }
        }
        if (Hp <= 0)
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("RedBullet"))
        {
            Hp -= other.GetComponent<BulletMove>().Damage;
        }
    }
}



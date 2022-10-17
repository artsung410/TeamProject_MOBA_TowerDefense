using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedShotRange : MonoBehaviour
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
            if (col.CompareTag("RedMinion"))
            {
                continue;
            }

            if (col.CompareTag("BlueMinion"))
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
        if (other.CompareTag("BlueBullet"))
        {
            Hp -= other.GetComponent<BulletMove>().Damage;
        }
    }


}

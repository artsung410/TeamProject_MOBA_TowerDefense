using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedShotRange : MonoBehaviour
{
    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################

    int maxHp = 100;
    int currentHp = 100;
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
        
        if (currentHp <= 0)
        {

            currentHp = 0;
            Destroy(transform.parent.gameObject);
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BlueBullet"))
        {
            currentHp -= other.GetComponent<BulletMove>().Damage;
            Destroy(other.gameObject);
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlueAttackRange : MonoBehaviour
{
    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################
    
    EnemySatatus Pistion;
    int Hp = 100;

    Slider _slider;
    private void Awake()
    {
        Pistion = GetComponent<EnemySatatus>();
        _slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] Target = Physics.OverlapSphere(transform.position, 5f);
           
        foreach (Collider col in Target)
        {
            if(col.CompareTag("BlueMinion"))
            {
                continue;
            }

            if (col.CompareTag("RedMinion"))
            {
                Pistion._target = col.gameObject.transform;
               
            }

        }
        if(Hp <= 0)
        {
            Destroy(gameObject);
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RedBullet"))
        {
            Hp -= other.GetComponent<BulletMove>().Damage;
        }
    }


}




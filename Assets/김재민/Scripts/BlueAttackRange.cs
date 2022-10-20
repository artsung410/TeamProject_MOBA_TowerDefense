using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class BlueAttackRange : MonoBehaviourPun
{
    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################
    
    EnemySatatus Pistion;
   public int maxHp { get; private set; }
   public int currentHp { get; private set; }

    private void Awake()
    {
        maxHp = 100;
        currentHp = 100;
        Pistion = GetComponent<EnemySatatus>();
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
        if(currentHp <= 0)
        {
            currentHp = 0;
            Destroy(transform.parent.gameObject);
        }
        
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("RedBullet"))
    //    {
    //        currentHp -= other.GetComponent<BulletMove>().Damage;
    //        Destroy(other.gameObject);
    //    }
    //}
   


}




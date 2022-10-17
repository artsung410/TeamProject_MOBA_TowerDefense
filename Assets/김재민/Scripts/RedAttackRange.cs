using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedAttackRange : MonoBehaviour
{
    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################
    
    EnemySatatus Pistion;
    Slider _slider;

    int maxHp = 100;
    int currentHp = 100;
    private void Awake()
    {
        Pistion = GetComponent<EnemySatatus>();
        _slider = GetComponentInChildren<Slider>();

    }

    // Update is called once per frame
    void Update()
    {
        cullectHp();
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


        if (currentHp <= 0)
        {
            currentHp = 0;
            Destroy(gameObject);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BlueBullet"))
        {
            currentHp -= other.GetComponent<BulletMove>().Damage;
        }
    }
    void cullectHp()
    {
        transform.position = Pistion.transform.position;
        _slider.value = Mathf.Lerp(_slider.value, currentHp / maxHp, Time.deltaTime * 5f);
    }


}
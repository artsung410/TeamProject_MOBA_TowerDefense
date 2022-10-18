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
    Slider _slider;
    private void Awake()
    {

        Shot = GetComponent<ShotEnemy>();
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

                Shot._target = col.gameObject.transform;

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
        _slider.transform.position = transform.position;
        _slider.value = Mathf.Lerp(_slider.value, currentHp / maxHp, Time.deltaTime * 1f);
    }
}


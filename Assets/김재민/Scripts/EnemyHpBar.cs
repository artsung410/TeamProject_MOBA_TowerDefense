using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class EnemyHpBar : MonoBehaviourPun
{
    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################
    // ±èÀç¹Î¹Ùº¸
    [SerializeField]
    private Transform Enemy;
    
    public Slider _slider;
    public EnemySatatus Enemy1;
    private void Awake()
    {
        _slider = GetComponent<Slider>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (Enemy1 == null)
        {
            return;
        }

        _slider.value = Enemy1.CurrnetHP / Enemy1.HP;
        transform.position = new Vector3(Enemy.position.x, 3f, Enemy.position.z);
    }
}
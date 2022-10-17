using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public Slider enemySlider3D;
    public static EnemyHealth instance;

    //public int health;
    Stats _enemyStats;


    private void Awake()
    {
        instance = this;
        _enemyStats = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Stats>();
    }

    void Start()
    {
        enemySlider3D.maxValue = _enemyStats.maxHealth;
        _enemyStats.health = _enemyStats.maxHealth;
        enemySlider3D.value = _enemyStats.health;
    }

    private void Update()
    {
        //Debug.Log(_enemyStats.maxHealth);
        //Debug.Log(_enemyStats.health);


        enemySlider3D.value = _enemyStats.health;

        //SyncHealth();
    }

    /// <summary>
    /// 2d와 3d의 값을 같게함
    /// </summary>
    private void SyncHealth()
    {
        enemySlider3D.value = _enemyStats.health;
    }

    //public void DecreaseHealth(float value)
    //{
    //    enemySlider3D.value -= value;
    //}
}

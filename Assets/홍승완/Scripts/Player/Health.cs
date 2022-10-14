using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public Slider playerSlider3D;
    Slider playerSlider2D;

    //public int health;
    Stats _playerStats;


    private void Awake()
    {
        playerSlider2D = GetComponent<Slider>();
        _playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<Stats>();
    }

    void Start()
    {
        playerSlider2D.maxValue = _playerStats.maxHealth;

        playerSlider3D.maxValue = playerSlider2D.maxValue;
        _playerStats.health = playerSlider3D.maxValue;

    }

    private void Update()
    {
        SyncHealth();
    }

    /// <summary>
    /// 2d와 3d의 값을 같게함
    /// </summary>
    private void SyncHealth()
    {
        playerSlider2D.value = _playerStats.health;
        playerSlider3D.value = playerSlider2D.value;
    }
}

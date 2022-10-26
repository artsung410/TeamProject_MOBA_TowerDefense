using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Health : MonoBehaviourPun
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public Slider hpSlider3D;
    Stats _stats;
    PlayerAnimation ani;

    float health;
    public bool isDeath
    {
        get;
        private set;
    }



    private void Awake()
    {
        _stats = GetComponent<Stats>();
        ani = GetComponent<PlayerAnimation>();
    }

    private void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        isDeath = false;

        health = _stats.StartHealth;

        hpSlider3D.maxValue = _stats.StartHealth;
        hpSlider3D.value = health;

    }

    private void Start()
    {

    }

    [PunRPC]
    public void HealthUpdate(float damage)
    {
        health = Mathf.Max(health - damage, 0);
        hpSlider3D.value = health;
        Die();
    }

    //[PunRPC]
    public void OnDamage(float damage)
    {
        photonView.RPC(nameof(HealthUpdate), RpcTarget.All, damage);
    }

    public void Die()
    {
        if (health <= 0f)
        {
            isDeath = true;
        }

        // 죽어있으니까 일단 없어짐
        if (isDeath)
        {
            //ani.DieMotion();
            gameObject.SetActive(false);
            hpSlider3D.gameObject.SetActive(false);
        }
    }

}

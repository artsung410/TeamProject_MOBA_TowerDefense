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

    float health;

    private void Awake()
    {
        _stats = GetComponent<Stats>();
    }

    private void OnEnable()
    {
    }

    private void Start()
    {
        health = _stats.StartHealth;

        hpSlider3D.maxValue = _stats.StartHealth;

        hpSlider3D.value = health;

    }

    private void Update()
    {
        
    }

    [PunRPC]
    public void HealthUpdate(float damage)
    {
        health = Mathf.Max(health - damage, 0);
        hpSlider3D.value = health;
    }


    //[PunRPC]
    public void OnDamage(float damage)
    {
        //other.GetComponent<PhotonView>().RPC("HealthUpdate", RpcTarget.AllBuffered, damage);
        photonView.RPC(nameof(HealthUpdate), RpcTarget.All, damage);
    }
}

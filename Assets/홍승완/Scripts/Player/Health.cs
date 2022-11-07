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
    WaitForSeconds Delay100 = new WaitForSeconds(1f);
    float overhp;

    private bool Maxhp = false;

    [HideInInspector]
    public float health;
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
        if (isDeath)
        {
            StopCoroutine(temp());
            return;
        }

        photonView.RPC(nameof(HealthUpdate), RpcTarget.All, damage);

    }

    public void Die()
    {
        if (health <= 0f)
        {
            PlayerHUD.Instance.AddScoreToEnemy(gameObject.tag);
            isDeath = true;
            ani.DieMotion();
            hpSlider3D.gameObject.SetActive(false);

            StartCoroutine(temp());
        }
    }

    IEnumerator temp()
    {
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }

    public void Regenation(float recovery)
    {
        photonView.RPC("PRC_regeneration", RpcTarget.All, recovery);

    }
    
    
    [PunRPC]
    private void PRC_regeneration(float recovery)
    { 

        health += recovery; // health «ˆ¿Á √º∑¬
        if (health >= _stats.StartHealth)
        {
            overhp = health - _stats.StartHealth;
            health -= overhp; // ∏∆Ω∫ √º∑¬¿∏∑Œ πŸ≤„¡‹
        }
        hpSlider3D.value = health;
        Debug.Log($"health : {health} ");
    }


}




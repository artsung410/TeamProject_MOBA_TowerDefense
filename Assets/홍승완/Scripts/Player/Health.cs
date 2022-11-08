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

    public float health;

    private float _maxHealth;
    private float _prevMaxHealth;
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

    }

    private void Start()
    {
        isDeath = false;
        _maxHealth = _stats.MaxHealth;
        _prevMaxHealth = _maxHealth;

        // 초기화 부분은 최대체력으로
        health = _maxHealth;

        hpSlider3D.maxValue = _maxHealth;
        hpSlider3D.value = health;
    }


    [PunRPC]
    public void HealthUpdate(float maxHP)
    {
        _maxHealth = maxHP;

        // 현재 나의 체력에서 증가된 체력량 만큼 조금 채워준다
        health += (_maxHealth - _prevMaxHealth);
        hpSlider3D.value = health;

        // 다시 preveMaxHealth를 최신화한다
        _prevMaxHealth = _maxHealth;

        hpSlider3D.maxValue = _maxHealth;
    }

    // 데미지를 입을때 hp가 감소되는 메서드
    [PunRPC]
    public void ReduceHealthPoint(float damage)
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
            StopCoroutine(DelayDisapearBody());
            return;
        }

        photonView.RPC(nameof(ReduceHealthPoint), RpcTarget.All, damage);

    }

    public void Die()
    {
        if (health <= 0f)
        {
            PlayerHUD.Instance.AddScoreToEnemy(gameObject.tag);
            isDeath = true;
            ani.DieMotion();
            hpSlider3D.gameObject.SetActive(false);

            StartCoroutine(DelayDisapearBody());
        }
    }

    IEnumerator DelayDisapearBody()
    {
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }

}

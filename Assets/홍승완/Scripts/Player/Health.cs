using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System;

public class Health : MonoBehaviourPun
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public static event Action<GameObject, float> OnPlayerDieEvent = delegate { }; // 구독자를 담을 배열?!

    public Slider hpSlider3D;
    Outline _outline;
    Stats _stats;
    public PlayerAnimation ani;
    WaitForSeconds Delay100 = new WaitForSeconds(1f);
    float overhp;

    private bool Maxhp = false;

    public float health;

    private float _maxHealth;
    private float _prevMaxHealth;
    public bool isDeath;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
        _stats = GetComponent<Stats>();
       
    }

    private void OnEnable()
    {
        _outline.enabled = false;

        Init();
    }


    private void Init()
    {
        isDeath = false;
        _maxHealth = _stats.MaxHealth;
        _prevMaxHealth = _maxHealth;
        health = _maxHealth;
        hpSlider3D.maxValue = _maxHealth;
        hpSlider3D.value = health;
    }

    private void Start()
    {
        isDeath = false;
        _maxHealth = _stats.MaxHealth;
        _prevMaxHealth = _maxHealth;

        // 초기화 부분은 최대체력으로
        health = _maxHealth;
        //Debug.Log("start호출");

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
            //StopCoroutine(DelayDisapearBody());
            return;
        }

        photonView.RPC(nameof(ReduceHealthPoint), RpcTarget.All, damage);

    }

    float exp = 10000f;


    public void Die()
    {
        if (health <= 0f)
        {
            PlayerHUD.Instance.AddScoreToEnemy(gameObject.tag);
            isDeath = true;
            ani.DieMotion();
            hpSlider3D.gameObject.SetActive(false);
            // 죽었을때 Invoke => 실행이 된다
            OnPlayerDieEvent.Invoke(this.gameObject, _stats.enemyExp);
            //StartCoroutine(DelayDisapearBody());
            gameObject.SetActive(false);
        }
    }

    // TODO : 플레이어 콜라이더 제거부분과 렌더러 제거부분 시간 다르게 적용;
    // 콜라이더 제거 => 렌더러 제거 => 게임 오브젝트 제거

    public void Regenation(float recovery)
    {
        photonView.RPC("PRC_regeneration", RpcTarget.All, recovery);
    }
    
    
    [PunRPC]
    private void PRC_regeneration(float recovery)
    { 

        health += recovery; // health 현재 체력
        if (health >= _stats.MaxHealth)
        {
            overhp = health - _stats.MaxHealth;
            health -= overhp; // 맥스 체력으로 바꿔줌
        }
        hpSlider3D.value = health;
        //Debug.Log($"health : {health} ");
    }


    public void DamageOverTime(float Damage,float Time)
    {
        photonView.RPC("RPC_DamageOverTime",RpcTarget.All,Damage,Time);
    }

    [PunRPC]
    private IEnumerator RPC_DamageOverTime(float Damage, float Time)
    {
        while (true)
        {
            if(Time <= 0)
            {
                yield  break;
            }
            health -= Damage;
            yield return Delay100;
            Time -= 1f;

            yield return null;
        }
    }

    private void OnMouseEnter()
    {
        if (photonView.IsMine) // 자기 자신이면 켜주고  색 그린
        {
            _outline.enabled = true; // 켜주고
            _outline.OutlineColor = Color.blue;
        }
        else
        {
            _outline.enabled = true;
            _outline.OutlineColor = Color.red;
        }

    }

    private void OnMouseExit()
    {
        _outline.enabled = false;
    }

}




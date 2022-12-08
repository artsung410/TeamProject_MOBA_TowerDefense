//#define OLD_VER
#define NEW_VER

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

#if NEW_VER
    public GameObject HealthObject;
    public Sprite[] sprites = new Sprite[3];
    public Image BackHealthBar;
    public Image FrontHealthBar;

#endif

#if OLD_VER
    public Slider hpSlider3D;

#endif
    Outline _outline;
    Stats _stats;
    public PlayerAnimation ani;
    WaitForSeconds Delay100 = new WaitForSeconds(1f);
    float overhp;

   
    private bool Maxhp = false;

    public float health;
    public float MaxHealth;

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
        MaxHealth = _stats.maxHealth;
        _prevMaxHealth = MaxHealth;
        health = MaxHealth;

#if OLD_VER

        hpSlider3D.maxValue = _maxHealth;
        hpSlider3D.value = health;
#endif

#if NEW_VER
        if (!photonView.IsMine)
        {
            BackHealthBar.sprite = sprites[0];
            FrontHealthBar.sprite = sprites[1];
        }
        else
        {
            BackHealthBar.sprite = sprites[1];
            FrontHealthBar.sprite = sprites[2];
        }
        FrontHealthBar.fillAmount = health / MaxHealth;
        BackHealthBar.fillAmount = FrontHealthBar.fillAmount;
        
#endif

    }

    private void Start()
    {
#if OLD_VER
        isDeath = false;
        _maxHealth = _stats.MaxHealth;
        _prevMaxHealth = _maxHealth;

        // 초기화 부분은 최대체력으로
        health = _maxHealth;
        //Debug.Log("start호출");

        //hpSlider3D.maxValue = _maxHealth;
        //hpSlider3D.value = health;

#endif
        Init();
    }

    float lerpTime;
    private void FixedUpdate()
    {
        float backFill = BackHealthBar.fillAmount;
        float frontFill = FrontHealthBar.fillAmount;
        if (backFill > frontFill)
        {
            lerpTime += Time.deltaTime;
            if (lerpTime >= 1)
            {
                lerpTime = 0f;
            }
            float test = lerpTime / 2;
            test = test * test;
            BackHealthBar.fillAmount = Mathf.Lerp(backFill, FrontHealthBar.fillAmount, test);
        }
    }




    [PunRPC]
    public void LevelHealthUpdate(float maxHP)
    {
#if OLD_VER
        _maxHealth = maxHP;

        // 현재 나의 체력에서 증가된 체력량 만큼 조금 채워준다
        health += (_maxHealth - _prevMaxHealth);

        hpSlider3D.value = health;

        // 다시 prevMaxHealth를 최신화한다
        _prevMaxHealth = _maxHealth;

        hpSlider3D.maxValue = _maxHealth;

#endif

#if NEW_VER
        MaxHealth = maxHP;
        health += (MaxHealth - _prevMaxHealth);
        FrontHealthBar.fillAmount = health / MaxHealth;
        _prevMaxHealth = MaxHealth;
#endif
    }

    // 데미지를 입을때 hp가 감소되는 메서드
    [PunRPC]
    public void ReduceHealthPoint(float damage)
    {
#if OLD_VER
        health = Mathf.Max(health - damage, 0);
        hpSlider3D.value = health;
        Die();

#endif

#if NEW_VER
        health = Mathf.Max(health - damage, 0);
        FrontHealthBar.fillAmount = health / MaxHealth;
        Die();
#endif
    }


    public void OnDamage(float damage)
    {
        if (isDeath)
        {
            return;
        }
        lerpTime = 0f;
        photonView.RPC(nameof(ReduceHealthPoint), RpcTarget.All, damage);

    }


    public void Die()
    {
        if (health <= 0f)
        {
            isDeath = true;
            PlayerHUD.Instance.AddScoreToEnemy(gameObject.tag);
            ani.DieMotion();
#if NEW_VER
            HealthObject.SetActive(false);
#endif
            // 죽었을때 Invoke => 실행이 된다
            OnPlayerDieEvent.Invoke(this.gameObject, _stats.enemyExp);
            gameObject.SetActive(false);

        }
    }

    public void Regenation(float recovery)
    {
        // TODO : 포톤에러로 인한 임시 주석처리
        photonView.RPC(nameof(PRC_regeneration), RpcTarget.All, recovery);
    }
    
    
    [PunRPC]
    private void PRC_regeneration(float recovery)
    { 

        health += recovery; // health 현재 체력
        if (health >= _stats.maxHealth)
        {
            overhp = health - _stats.maxHealth;
            health -= overhp; // 맥스 체력으로 바꿔줌
        }
#if OLD_VER
        hpSlider3D.value = health;

#endif

#if NEW_VER
        FrontHealthBar.fillAmount = health / MaxHealth;
        //elapseBar.fillAmount = healthBar.fillAmount;
#endif
    }


    public void DamageOverTime(float Damage,float Time)
    {
        photonView.RPC(nameof(RPC_DamageOverTime),RpcTarget.All,Damage,Time);
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
            //health -= Damage;
            OnDamage(Damage);
            yield return Delay100;
            Time -= 1f;

            yield return null;
        }
    }

    private void OnMouseEnter()
    {
        if (photonView.IsMine) // 자기 자신이면 켜주고  색 그린
        {
            // 마우스 커서 변경 코드
            Cursor.SetCursor(PlayerHUD.Instance.cursorMoveAlly, Vector2.zero, CursorMode.Auto);
            _outline.enabled = true; // 켜주고
            _outline.OutlineColor = Color.blue;
        }
        else
        {
            Cursor.SetCursor(PlayerHUD.Instance.cursorMoveEnemy, Vector2.zero, CursorMode.Auto);
            _outline.enabled = true;
            _outline.OutlineColor = Color.red;
        }

    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(PlayerHUD.Instance.cursorMoveNamal, Vector2.zero, CursorMode.Auto);
        _outline.enabled = false;
    }

}




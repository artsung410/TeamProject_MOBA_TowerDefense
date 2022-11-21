using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class SkillHandler : MonoBehaviourPun, IDamageable
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    protected HeroAbility _ability;
    protected Stats _stat;
    protected PlayerBehaviour _behaviour;
    protected PlayerAnimation _ani;
    protected string _myTag;
    
    // TODO : skilldata scriptableObj사용할수있게 개조하기
    //protected ScriptableObject data;


    public float SetDamage;
    public float SetHodingTime;
    public float SetRange;
    public float SetLockTime;

    // 플레이어 HeroAbility를 받아옴
    public void GetPlayerPos(HeroAbility heroAbility)
    {
        _ability = heroAbility;
    }

    // 플레이어 Stats을 받아옴
    public void GetPlayerStatus(Stats stats)
    {
        _stat = stats;
    }

    // 플레이어 PlayerBehaviour를 받아옴
    public void GetMousePos(PlayerBehaviour behaviour)
    {
        _behaviour = behaviour;
    }

    // 플레이어 Animation 받아옴
    public void GetAnimation(PlayerAnimation animation)
    {
        _ani = animation;
    }

    public void SkillDamage(float damage, GameObject target)
    {
        if (target.gameObject.layer == 7)
        {
            Health player = target.GetComponent<Health>();

            if (player != null)
            {
                player.OnDamage(damage);

            }
        }
        else if (target.gameObject.layer == 8 || target.gameObject.layer == 13 || target.gameObject.layer == 17)
        {
            Enemybase minion = target.GetComponent<Enemybase>();

            if (minion != null)
            {
                minion.TakeDamage(damage);
            }
        }
    }

    public void SkillTimeDamage(float damage,float time ,GameObject target) // .데미지 지속시간 , 타켓
    {
        if (target.gameObject.layer == 7)
        {
            Health player = target.GetComponent<Health>();

            if (player != null)
            {
                Debug.Log("플레이어 지속데미지 적용 시작");
                player.DamageOverTime(damage,time);
            }
        }
        else if (target.gameObject.layer == 8 || target.gameObject.layer == 13)
        {
            Enemybase minion = target.GetComponent<Enemybase>();


            if (minion != null)
            {
                Debug.Log("input skillTimeDamage");
                minion.DamageOverTime(damage,time);
            }
        }
    }

    public void CrowdControlStun(GameObject target, float time, bool stun)
    {
        if (target.gameObject.layer == 7)
        {
            PlayerBehaviour player = target.GetComponent<PlayerBehaviour>();

            if (player != null)
            {
                player.OnStun(stun, time);
            }
        }
    }

    protected string GetMytag(HeroAbility ability)
    {
        _myTag = ability.tag;
        return _myTag;

    }

    public abstract void SkillHoldingTime(float time);

    public abstract void SkillUpdatePosition();

    // TODO : 검기 위치 재조정

}

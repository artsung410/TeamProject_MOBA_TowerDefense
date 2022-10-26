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

    public float SetDamage;

    public abstract void SkillHoldingTime(float time);


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

    public abstract void SkillDamage(float damage, GameObject target);
}

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


    // �÷��̾� HeroAbility�� �޾ƿ�
    public void GetPlayerPos(HeroAbility heroAbility)
    {
        _ability = heroAbility;
    }

    // �÷��̾� Stats�� �޾ƿ�
    public void GetPlayerStatus(Stats stats)
    {
        _stat = stats;
    }

    // �÷��̾� PlayerBehaviour�� �޾ƿ�
    public void GetMousePos(PlayerBehaviour behaviour)
    {
        _behaviour = behaviour;
    }

    public abstract void SkillDamage(float damage, GameObject target);
}

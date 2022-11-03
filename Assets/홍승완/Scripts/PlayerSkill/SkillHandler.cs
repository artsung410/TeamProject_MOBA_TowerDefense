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
    public float SetHodingTime;
    public float SetRange;

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
        else if (target.gameObject.layer == 8 || target.gameObject.layer == 13)
        {
            Enemybase minion = target.GetComponent<Enemybase>();


            if (minion != null)
            {
                minion.TakeDamage(damage);
            }
        }
    }

    public abstract void SkillHoldingTime(float time);

    public abstract void SkillUpdatePosition();
    
}

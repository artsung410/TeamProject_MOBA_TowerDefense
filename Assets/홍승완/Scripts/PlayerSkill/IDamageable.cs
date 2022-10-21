using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    void TakeDamage(float damage);

    void SkillHoldingTime(float time);

    void SkillCoolTime(float time);
   
}

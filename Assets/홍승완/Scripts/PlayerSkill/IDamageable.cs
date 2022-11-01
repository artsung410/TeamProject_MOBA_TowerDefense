using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    /// <summary>
    /// 스킬의 지속시간
    /// </summary>
    /// <param name="time">지속시간</param>
    void SkillHoldingTime(float time);



    /// <summary>
    /// 스킬의 데미지 담당 메서드
    /// </summary>
    /// <param name="damage">스킬의 데미지</param>
    /// <param name="target">데미지 줄 타겟, 해당 타겟에겐 체력이 존재해야한다</param>
    void SkillDamage(float damage, GameObject target);

    void SkillUpdatePosition();


}

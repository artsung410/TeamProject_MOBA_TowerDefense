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
    /// ��ų�� ���ӽð�
    /// </summary>
    /// <param name="time">���ӽð�</param>
    void SkillHoldingTime(float time);



    /// <summary>
    /// ��ų�� ������ ��� �޼���
    /// </summary>
    /// <param name="damage">��ų�� ������</param>
    /// <param name="target">������ �� Ÿ��, �ش� Ÿ�ٿ��� ü���� �����ؾ��Ѵ�</param>
    void SkillDamage(float damage, GameObject target);

    void SkillUpdatePosition();


}

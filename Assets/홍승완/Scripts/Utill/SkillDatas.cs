using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "SkillName", menuName = "HongSkillDatas/Create New HongSkillDatas")]
public class SkillDatas : ScriptableObject
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public List<PlayerSkillDatas> DataList = new List<PlayerSkillDatas>();

}

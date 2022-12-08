using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "StatName", menuName = "HongStatDatas/Create New HongStatDatas")]
public class StatDatas : ScriptableObject
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public List<PlayerStatDatas> dataList = new List<PlayerStatDatas>();
}

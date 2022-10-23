using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com
//             ±âÈ¹DB -> (4)Effect_Table Àû¿ë
// ###############################################
[System.Serializable]
[CreateAssetMenu(fileName = "BuffName", menuName = "Buff/Create New Buff")]
public class BuffData : ScriptableObject
{
    public enum TargetType
    {
        Player,
        Minion,
        Tower
    }

    public int id;
    public string Name;
    [TextArea] public string Desc;
    public int Effect_Type;
    public int Effect_Value;
    public bool Unlimited;
    public float Effect_Duration;
    public TargetType targetType;
    public Sprite buffIcon;
}


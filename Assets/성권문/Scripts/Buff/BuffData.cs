using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Target
{
    Player,
    Enemy,
    My_Minion,
    Enemy_Minion,
    Whole_Enemy,
    Tower
}
// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com
//             ±âÈ¹DB -> Effect_Table Àû¿ë
// ###############################################
[System.Serializable]
[CreateAssetMenu(fileName = "BuffName", menuName = "BuffData/Create New BuffData")]
public class BuffData : ScriptableObject
{
    public enum Effect_Type
    {
        Damage,
        Buff,
        Debuff,
        Minion,
    }

    public enum Effect_Position
    {
        Player,
        Enemy
    }

    [SerializeField]
    private int _id;
    public int Id { get { return _id; } }

    [SerializeField]
    private string _name;
    public string Name { get { return _name; } }

    [SerializeField]
    private Effect_Type _effectType;
    public Effect_Type EffectType { get { return _effectType; } }

    [SerializeField]
    private Target _target;
    public Target TargetType { get { return _target; } }

    [SerializeField]
    private float _effectValue;
    public float EffectValue { get { return _effectValue; } }

    [SerializeField]
    private Effect_Position _effectPosition;
    public Effect_Position EffectPosition { get { return _effectPosition; } }

    [SerializeField]
    private bool _unlimited;
    public bool Unlimited { get { return _unlimited; } }

    [SerializeField]
    private float _effectDuration;
    public float EffectDuration { get { return _effectDuration; } }

    [SerializeField]
    [TextArea] private string _desc;
    public string Desc { get { return _desc; } }

    [SerializeField]
    private Sprite _buffIcon;
    public Sprite BuffIcon { get { return _buffIcon; } }
}


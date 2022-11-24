using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com
//             ±âÈ¹DB -> Effect_Table Àû¿ë
// ###############################################
[System.Serializable]
[CreateAssetMenu(fileName = "BuffName", menuName = "BuffData/Create New BuffData")]
public class BuffData : ScriptableObject
{
    [SerializeField]
    private int _id;
    public int Id { get { return _id; } }

    [SerializeField]
    private int _group_id;
    public int Group_ID { get { return _group_id; } }

    [SerializeField]
    private string _name;
    public string Name { get { return _name; } }

    //[SerializeField]
    //private Buff_Effect_Type _effectType;
    //public Buff_Effect_Type EffectType { get { return _effectType; } }

    [SerializeField]
    private Buff_Target _target;
    public Buff_Target TargetType { get { return _target; } }

    [SerializeField]
    private LayerMask _layer;
    public LayerMask LayerType { get { return _layer; } }

    [SerializeField]
    private float _effectValue;
    public float EffectValue { get { return _effectValue; } }

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


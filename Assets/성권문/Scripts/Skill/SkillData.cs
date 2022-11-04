using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com
//             ±âÈ¹DB -> Skill_Table Àû¿ë
// ###############################################

public enum Crowd_Control_Type
{
    Normal,
    Slow,
    Stun,
}

[System.Serializable]
[CreateAssetMenu(fileName = "SkillName", menuName = "SkillData/Create New SkillData")]
public class SkillData : ScriptableObject
{
    [SerializeField]
    private int _id;
    public int Id { get { return _id; } }

    [SerializeField]
    private string _name;
    public string Name { get { return _name; } }

    [SerializeField]
    private string _name_Level;
    public string Name_Level { get { return _name_Level; } }

    [SerializeField]
    private int _Classification;
    public int Classification { get { return _Classification; } }

    [SerializeField]
    private int _rank;
    public int Rank { get { return _rank; } }

    [SerializeField]
    private int _Type;
    public int Type { get { return _Type; } }

    [SerializeField]
    private int _value1;
    public int Value1 { get { return _value1; } }

    [SerializeField]
    private int _value2;
    public int Value2 { get { return _value2; } }

    [SerializeField]
    private float _cool_Time;
    public float Cool_Time { get { return _cool_Time; } }

    [SerializeField]
    private float _range;
    public float Range { get { return _range; } }

    [SerializeField]
    private int _range_Type;
    public int Range_Type { get { return _range_Type; } }

    [SerializeField]
    private int _range_Value1;
    public int Range_Value1 { get { return _range_Value1; } }

    [SerializeField]
    private int _range_Value2;
    public int Range_Value2 { get { return _range_Value2; } }

    [SerializeField]
    private float _holding_Time;
    public float Holding_Time { get { return _holding_Time; } }

    [SerializeField]
    private int _continuous_Damage;
    public int Continuous_Damage { get { return _continuous_Damage; } }

    [SerializeField]
    private int _continuous_Damage_Hits;
    public int Continuous_Damage_Hits { get { return _continuous_Damage; } }

    [SerializeField]
    private int _continuous_Damage_Time;
    public int Continuous_Damage_Time { get { return _continuous_Damage_Time; } }

    [SerializeField]
    private Crowd_Control_Type _crowd_Control_Type;
    public Crowd_Control_Type Crowd_Control_Type { get { return _crowd_Control_Type; } }

    [SerializeField]
    private float _crowd_Control_Time;
    public float Crowd_Control_Time { get { return _crowd_Control_Time; } }

    [SerializeField]
    private float _crowd_Control_Value;
    public float Crowd_Control_Value { get { return _crowd_Control_Time; } }

    [SerializeField]
    private GameObject _objectPF;
    public GameObject ObjectPF { get { return _objectPF; } }

    [SerializeField]
    private BuffData _buff;
    public BuffData Buff { get { return _buff; } }

    [SerializeField]
    private Sprite _icon;
    public Sprite Icon { get { return _icon; } }
}

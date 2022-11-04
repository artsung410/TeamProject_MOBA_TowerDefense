using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com
//             ±âÈ¹DB -> Effect_Table Àû¿ë
// ###############################################

public enum Minion_Type
{
    Normal,
    Range,
    Special,
    Skill
}

[System.Serializable]
[CreateAssetMenu(fileName = "MinionName", menuName = "MinionData/Create New MinionData")]
public class MinionData : ScriptableObject
{
    [SerializeField]
    private int _id;
    public int Id { get { return _id; } }

    [SerializeField]
    private string _name;
    public string Name { get { return _name; } }

    [SerializeField]
    private Minion_Type _minion_Type;
    public Minion_Type Minion_Type { get { return _minion_Type; } }

    [SerializeField]
    private int _target_Recognize_Range;
    public int Target_Recognize_Range { get { return _target_Recognize_Range; } }

    [SerializeField]
    private float _attack;
    public float Attck { get { return _attack; } }

    [SerializeField]
    private float _attack_Speed;
    public float Attack_Speed { get { return _attack_Speed; } }

    [SerializeField]
    private float _attack_Range;
    public float Attack_Range { get { return _attack_Range; } }

    [SerializeField]
    private float _move_Speed;
    public float Move_Speed { get { return _move_Speed; } }

    [SerializeField]
    private float _hp;
    public float Hp { get { return _hp; } }

    [SerializeField]
    private float _exp;
    public float Exp { get { return _exp; } }

    [SerializeField]
    private Sprite _icon;
    public Sprite Icon { get { return _icon; } }

    [SerializeField]
    [TextArea] private string _desc;
    public string Desc { get { return _desc; } }
}


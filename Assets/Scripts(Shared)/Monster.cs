using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "MonsterName", menuName = "MonsterData/Create New MonsterData")]
public class Monster : ScriptableObject
{
    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################

    [SerializeField]
    private int _group_id;
    public int GroupId { get { return _group_id; } }
    [SerializeField]
    private int _attackDamage;
    public int AttackDamage { get { return _attackDamage; } }
    [SerializeField]
    private int _attackSpeed;
    public int AttackSpeed { get { return _attackSpeed; } }
    [SerializeField]
    private float _range;
    public float Range { get { return _range; } }
    [SerializeField]
    private int _moveSpeed;
    public int MoveSpeed { get { return _moveSpeed; } }
    [SerializeField]
    private float _hp;
    public float Hp { get { return _hp; } }
    [SerializeField]
    private float _exp;
    public float Exp { get { return _exp; } }
    [Header("≈¯∆¡ æ∆¿Ãƒ‹")]
    [SerializeField]
    private Sprite _icon;
    public Sprite Icon { get { return _icon; } }

}

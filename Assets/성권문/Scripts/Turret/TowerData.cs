using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com
//             ±âÈ¹DB -> (4)Effect_Table Àû¿ë
// ###############################################
[System.Serializable]
[CreateAssetMenu(fileName = "TowerName", menuName = "TowerData/Create New TowerData")]
public class TowerData : ScriptableObject
{
    public enum Tower_Type
    {
        Attack_Tower,
        Buff_Tower,
        DeBuff_Tower,
        Minion_Tower
    }

    public enum Attack_Range_Type
    {
        Circle,
        Square,
        Sector,
        Straight,
        Single
    }

    [SerializeField]
    private int _id;
    public int Id { get { return _id; } }

    [SerializeField]
    private string _name;
    public string Name { get { return _name; } }

    [SerializeField]
    private Tower_Type _towerType;
    public Tower_Type TowerType { get { return _towerType; } }

    [SerializeField]
    private float _attack;
    public float Attack { get { return _attack; } }

    [SerializeField]
    private float _attackSpeed;
    public float AttackSpeed { get { return _attackSpeed; } }

    [SerializeField]
    private float _attackRange;
    public float AttackRange { get { return _attackRange; } }

    [SerializeField]
    private Attack_Range_Type _attackRangeType;
    public Attack_Range_Type AttackRangeType { get { return _attackRangeType; } }

    [SerializeField]
    private float _attackRangeTypeRadius;
    public float AttackRangeTypeRadius { get { return _attackRangeTypeRadius; } }

    [SerializeField]
    private float _maxHP;
    public float MaxHP { get { return _maxHP; } }


    [SerializeField]
    private List<ScriptableObject> _scriptables;
    public List<ScriptableObject> Scriptables { get { return _scriptables; } }

    [SerializeField]
    private GameObject _objectPF;
    public GameObject ObjectPF { get { return _objectPF; } }

    [SerializeField]
    private Sprite _icon;
    public Sprite Icon { get { return _icon; } }

    [SerializeField]
    [TextArea] private string _desc;
    public string Desc { get { return _desc; } }

    [SerializeField]
    private AudioClip _soundAttack;
    public AudioClip SoundAttack { get { return _soundAttack; } }

    [SerializeField]
    private AudioClip _soundHit;
    public AudioClip SoundHit { get { return _soundHit; } }

    [SerializeField]
    private AudioClip _soundNormal;
    public AudioClip SoundNormal { get { return _soundNormal; } }

}


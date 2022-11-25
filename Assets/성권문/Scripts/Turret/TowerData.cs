using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com
//             기획DB -> Tower_Table 적용
// ###############################################
[System.Serializable]
[CreateAssetMenu(fileName = "TowerName", menuName = "TowerData/Create New TowerData")]
public class TowerData : ScriptableObject
{
    public enum Attack_Range_Type
    {
        Circle,
        Square,
        Sector,
        Straight,
        Single
    }

    public enum Projectile_Attack_Type
    {
        Single,
        Splash
    }

    //[SerializeField]
    //private int Tower_Grade;
    //public int Group_ID { get { return _group_id; } }

    [SerializeField]
    private int _group_id;
    public int Group_ID { get { return _group_id; } }

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
    private float _maxHP;
    public float MaxHP { get { return _maxHP; } }

    [Header("보조DB")]
    [SerializeField]
    private List<ScriptableObject> _scriptables;
    public List<ScriptableObject> Scriptables { get { return _scriptables; } }

    [Header("오브젝트PF(미니언 타워만 해당)")]
    [SerializeField]
    private GameObject _objectBluePF;
    public GameObject ObjectBluePF { get { return _objectBluePF; } }
    [SerializeField]
    private GameObject _objectRedPF;
    public GameObject ObjectRedPF { get { return _objectRedPF; } }

    [Header("타워 파괴효과PF")]
    [SerializeField]
    private GameObject _destroyPF;
    public GameObject DestroyPF { get { return _destroyPF; } }

    [SerializeField]
    private float _destroySpeed;
    public float DestroySpeed { get { return _destroySpeed; } }

    [Header("툴팁 아이콘")]
    [SerializeField]
    private Sprite _icon;
    public Sprite Icon { get { return _icon; } }

    [Header("툴팁 설명창")]
    [SerializeField]
    [TextArea] private string _desc;
    public string Desc { get { return _desc; } }

    [Header("효과음")]
    [SerializeField]
    private AudioClip _soundAttack;
    public AudioClip SoundAttack { get { return _soundAttack; } }

    [SerializeField]
    private AudioClip _soundHit;
    public AudioClip SoundHit { get { return _soundHit; } }

    [SerializeField]
    private AudioClip _soundNormal;
    public AudioClip SoundNormal { get { return _soundNormal; } }

    [Header("투사체")]
    [SerializeField]
    private GameObject _projectiles;
    public GameObject Projectiles { get { return _projectiles; } }

    [SerializeField]
    private float _projectiles_MoveSpeed;
    public float Projectiles_MoveSpeed { get { return _projectiles_MoveSpeed; } }

    [SerializeField]
    private Projectile_Attack_Type _projectile_Attack_Type;
    public Projectile_Attack_Type ProjectileAttackType { get { return _projectile_Attack_Type; } }
}


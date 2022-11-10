using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Networking;

public enum Stat_Columns
{
    HP,
    Dmg,
    Range,
    Atk_Speed,
    Move_Speed,
    Min_Exp,
    Max_Exp,
}

public class Stats : GoogleSheetManager
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################
    [Header("ü�� ����")]
    public float MaxHealth = 1;
    //public float health;

    [Header("���� ����")]
    public float attackDmg = 1;
    public float attackSpeed = 1;
    public float attackRange = 1;

    // TODO : �̵��ӵ� ����, ����� �����ؼ� ���ο� ���� �߰��� �ʿ�����
    [Header("�̵� ����")]
    public float MoveSpeed = 1;

    [Header("����")]
    public int Level;

    [Header("����ġ")]
    public float Exp;
    public float ExpDetectRange;

    private float minExp;
    private float maxExp;

    PlayerBehaviour _playerScript;
    Health _health;

    private void Awake()
    {
        _playerScript = GetComponent<PlayerBehaviour>();
        _health = GetComponent<Health>();
        Debug.Log("Awake ����");
        StartCoroutine(GetLevelData());

        // ������ ���
        Health.OnPlayerDieEvent += PlayerLevelUpFactory;
        Enemybase.OnMinionDieEvent += PlayerLevelUpFactory;
        Turret.OnTurretDestroyEvent += PlayerLevelUpFactory;
    }

    IEnumerator GetLevelData()
    {
        UnityWebRequest GetWarriorData = UnityWebRequest.Get(WarriorURL);
        yield return GetWarriorData.SendWebRequest();
        SetWarriorStats(GetWarriorData.downloadHandler.text);

        // �ʱ�ȭ�� �ʹ� ���� => ó�� ���� �ʱ�ȭ�� ���� ���� ����ұ�?
        StatInit();
    }

    private void OnEnable()
    {
    }

    private void Start()
    {
        Level = 1;
        MaxHealth = 500;
        attackDmg = 10;
        attackRange = 5;
        attackSpeed = 1;
        MoveSpeed = 15;
        minExp = 0;
        maxExp = 100;

        ExpDetectRange = 20f;
    }

    public void StatInit()
    {
        Level = 1;

        MaxHealth = float.Parse(WarriorLevelData[Level][(int)Stat_Columns.HP]);
        Debug.Log($"�Ľ��� ü�� ��ġ : {MaxHealth}");
        attackDmg = float.Parse(WarriorLevelData[Level][(int)Stat_Columns.Dmg]) + 100f;
        attackRange = float.Parse(WarriorLevelData[Level][(int)Stat_Columns.Range]);
        attackSpeed = float.Parse(WarriorLevelData[Level][(int)Stat_Columns.Atk_Speed]);
        MoveSpeed = float.Parse(WarriorLevelData[Level][(int)Stat_Columns.Move_Speed]);
        minExp = float.Parse(WarriorLevelData[Level][(int)Stat_Columns.Min_Exp]);
        maxExp = float.Parse(WarriorLevelData[Level][(int)Stat_Columns.Max_Exp]);
    }



    private void Update()
    {

        if (photonView.IsMine)
        {
            //if (Input.GetKeyDown(KeyCode.I))
            //{
            //    int exp = 30;
            //    Debug.Log($"���� ����ġ : {exp}");
            //    PlayerLevelUpFactory(exp);
            //    //photonView.RPC(nameof(PlayerLevelUp), RpcTarget.All, exp);
            //}
            //if (Input.GetKeyDown(KeyCode.L))
            //{
            //    int exp = 3000;
            //    Debug.Log($"���� ����ġ : {exp}");
            //    PlayerLevelUpFactory(exp);
            //    //photonView.RPC(nameof(PlayerLevelUp), RpcTarget.All, exp);
            //}
        }
    }


    // ������ ���� ���� ����
    public void SetStats(int level)
    {
        MaxHealth = float.Parse(WarriorLevelData[level][(int)Stat_Columns.HP]);

        attackDmg = float.Parse(WarriorLevelData[level][(int)Stat_Columns.Dmg]);
        attackRange = float.Parse(WarriorLevelData[level][(int)Stat_Columns.Range]);
        attackSpeed = float.Parse(WarriorLevelData[level][(int)Stat_Columns.Atk_Speed]);

        MoveSpeed = float.Parse(WarriorLevelData[level][(int)Stat_Columns.Move_Speed]);

        minExp = float.Parse(WarriorLevelData[level][(int)Stat_Columns.Min_Exp]);
        maxExp = float.Parse(WarriorLevelData[level][(int)Stat_Columns.Max_Exp]);
    }

    [PunRPC]
    public void PlayerLevelUpFactory(GameObject expBag, float exp)
    {
        // expBag�� ���� tag�� ������ �������̴ϱ� return�Ѵ�
        if (expBag.tag == gameObject.tag)
        {
            return;
        }

        // expBag�� ������ �Ÿ��� ����Ѵ�
        float dist = Vector3.Distance(expBag.transform.position, this.transform.position);
        Debug.Log($"���� {expBag.name}�� ������ �Ÿ� : {dist}");
        
        // �Ÿ��� �νİ����� �Ÿ� ���� �ִٸ� ����ġ ����
        if (dist <= ExpDetectRange)
        {
            Exp += exp;
            // ����ġ�� �ִ� ����ġ���� ������ �������� �Ѵ�
            while (Exp >= maxExp)
            {
                // 10���� �޼��� �����������ʰ� ����ġ�ٴ� ���� �ִ�ġ �̻����� ���� �ʴ´�
                if (WarriorLevelData.ContainsKey(Level + 1) == false)
                {
                    Exp = Mathf.Clamp(Exp, minExp, maxExp);
                    return;
                }
                Level++;

                // Ÿ�� �ر��� ���ӸŴ����� �÷��̾� ������ �޾ƿͼ� �ر��Ѵ�
                GameManager.Instance.UnlockTower(Level);
                SetStats(Level);
                photonView.RPC(nameof(_health.HealthUpdate), RpcTarget.All, MaxHealth);

                // Exp���� maxExp��ŭ ���� �������� �����ϱ�
                Exp = Mathf.Max(Exp - maxExp, 0);
            }
        }


    }

    // TODO : ����������(overlapsphere)�� ���� ���(dieȣ���)�ҽ� �÷��̾�� ����ġ���ش�(��, ������ ����(tagó���Ұ�))

    public float tempExp;
    float rangeMinionExp = 30f;
    float meleeMinionExp = 40f;

    // �Ⱒ ���� : ���� �� �Ǻ��Ҷ� �����ؼ� ���� ����
    //IEnumerator ExpDetector()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(0.1f);
    //        Collider[] expBag = Physics.OverlapSphere(transform.position, ExpDetectRange);
    //        //Debug.Log($"�ֺ� �ݶ��̴��� : {expBag.Length}");
    //        if (expBag.Length > 0)
    //        {
    //            foreach (Collider _exps in expBag)
    //            {
    //                // ����� �ݶ��̴����� ������ü�� ���� �޾ƿ���
    //                // ���� ��ü�� [�÷��̾�]�� {100}
    //                // ���� ��ü�� [�ٰŸ� �̴Ͼ�]�̸� {40}
    //                // ���� ��ü�� [���Ÿ� �̴Ͼ�]�̸� {30}
    //                // ���� ��ü�� [Ư�� �̴Ͼ�]�̸� {50}
    //                // ���� ��ü�� [Ÿ��]�� {200}
    //                //Debug.Log($"�ֺ� �ݶ��̴� : {_exps.gameObject.name}");

    //                // �ݶ��̴��� ���� �˻�
    //                if (_exps.gameObject.tag == _playerScript.EnemyTag)
    //                {
    //                    // �� �÷��̾�
    //                    if (_exps.gameObject.GetComponent<Health>() != null)
    //                    {
    //                        Debug.Log("�� Ȯ��");
    //                        Health enemyPlayer = _exps.gameObject.GetComponent<Health>();
    //                        // ���� �׾�����
    //                        if (enemyPlayer.isDeath == true)
    //                        {
    //                            // ����ġ�� ��´�
    //                            tempExp += 30f;
    //                        }
    //                    }

    //                    if (_exps.gameObject.GetComponent<Enemybase>() != null)
    //                    {
    //                        Debug.Log("�� �̴Ͼ� Ȯ��");
    //                        Enemybase enemyMinion = _exps.gameObject.GetComponent<Enemybase>();

    //                        if (enemyMinion.isDead == true)
    //                        {
    //                            tempExp += 15f;
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.2f);
        Gizmos.DrawSphere(transform.position, ExpDetectRange);
    }


    private void OnDisable()
    {
        //Debug.Log("Stats disableȣ��");
        StopAllCoroutines();
    }
}
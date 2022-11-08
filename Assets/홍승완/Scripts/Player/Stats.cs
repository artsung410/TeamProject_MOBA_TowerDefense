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

    public int Level;

    public float Exp;

    private float minExp;
    private float maxExp;

    PlayerBehaviour _playerScript;
    Health _health;

    // dictinary�� ����� stat���� -> key���� ���� ����

    private void Awake()
    {
        _playerScript = GetComponent<PlayerBehaviour>();
        _health = GetComponent<Health>();

        StartCoroutine(GetLevelData());
    }

    IEnumerator GetLevelData()
    {
        UnityWebRequest GetWarriorData = UnityWebRequest.Get(WarriorURL);
        yield return GetWarriorData.SendWebRequest();
        SetWarriorStats(GetWarriorData.downloadHandler.text);

        // �ʱ�ȭ�� �ʹ� ���� => ó�� ���� �ʱ�ȭ�� ���� ���� ����ұ�?
        StatInit();
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
    }

    public void StatInit()
    {
        Level = 1;

        MaxHealth = float.Parse(WarriorLevelData[Level][(int)Stat_Columns.HP]);
        attackDmg = float.Parse(WarriorLevelData[Level][(int)Stat_Columns.Dmg]);
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
            if (Input.GetKeyDown(KeyCode.I))
            {
                int exp = 30;
                Debug.Log($"���� ����ġ : {exp}");
                PlayerLevelUp(exp);
                //photonView.RPC(nameof(PlayerLevelUp), RpcTarget.All, exp);
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                int exp = 3000;
                Debug.Log($"���� ����ġ : {exp}");
                PlayerLevelUp(exp);
                //photonView.RPC(nameof(PlayerLevelUp), RpcTarget.All, exp);
            }
        }

        //if (Level == 1 || Level == 3 || Level == 5 || Level == 7)
        //{
        //    // Ÿ�� �ϳ��� ����
        //    GameManager.Instance.UnlockTower();
        //}
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
    public void PlayerLevelUp(float exp)
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
            GameManager.Instance.UnlockTower(Level);
            SetStats(Level);
            photonView.RPC(nameof(_health.HealthUpdate), RpcTarget.All, MaxHealth);
            // Exp���� maxExp��ŭ ���� �������� �����ϱ�
            Exp = Mathf.Max(Exp - maxExp, 0);
        }

    }

    // TODO : ����ġ ������ �������?
    
    // ����������(overlapsphere)�� ���� ���(dieȣ���)�ҽ� �÷��̾�� ����ġ���ش�(��, ������ ����(tagó���Ұ�))

    // TODO : 1/3/5/7������ Ÿ���� �ϳ��� �ر�(?)�ȴ�

}
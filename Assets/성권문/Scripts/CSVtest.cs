using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System;

public enum SkillColData
{
    ID = 0,
    Name,
    NameLevel,
    Probability,
    Classification,
    Rank,
    LockTime,
    SkillType,
    Value1,
    Value2,
    CoolTime,
    Range,
    RangeType,
    RangeValue1,
    RangeValue2,
    HoldingTime,
    TickDamage,
    TickCount,
    TickTime,
    CcType,
    CcTime,
    CcValue,
    CardImage,
    SkillIcon,
    Desc,
}

public enum DescColData
{
    ID,
    Name,
    Text_Ko,
    Text_En,
}

public enum StatColData
{
    ID,             // int
    Name,           // string
    NameLevel,      // string
    HP,             // float
    Dmg,            // float
    Range,          // float
    Atk_Speed,      // float
    Move_Speed,     // float
    Max_Exp,        // int
    Character_ID,   // int
    Exp_Enemy,      // int   
}


public class CSVtest : MonoBehaviour
{
    private static CSVtest _instance;
    public static event Action onDataParsingEvent = delegate { };
    public static CSVtest Instance
    {
        get
        {
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(CSVtest)) as CSVtest;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        // 인스턴스가 존재하는 경우 새로생기는 인스턴스를 삭제한다.
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        // 아래의 함수를 사용하여 씬이 전환되더라도 선언되었던 인스턴스가 파괴되지 않는다.
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartCoroutine(GetDescData(descURL));
    }



    #region DescParsing

    private const string descURL = "https://docs.google.com/spreadsheets/d/182kN9I20M15JBWPLeWa-TvWvAjKiKSyEfA9sq9FsMPY/export?format=tsv&gid=54124976&range=A3:D109";

    Dictionary<int, List<string>> descDic = new Dictionary<int, List<string>>();
    List<List<string>> descList = new List<List<string>>();



    IEnumerator GetDescData(string url)
    {
        UnityWebRequest DescDataRequest = UnityWebRequest.Get(url);
        yield return DescDataRequest.SendWebRequest();
        SplitDescData(DescDataRequest.downloadHandler.text);

        StartCoroutine(GetTowerData());
        StartCoroutine(GetBuffData());
        StartCoroutine(GetMinionData());

        StartCoroutine(GetWarriorSkillData(warriorSkillURL));
        StartCoroutine(GetWizardSkillData(wizardSkillURL));
        StartCoroutine(GetCommonSkillData(commonSkillURL));

        StartCoroutine(GetWarriorStatData(warriorStatURL));
        StartCoroutine(GetWizardStatData(wizardStatURL));
    }

    private void SplitDescData(string tsv)
    {
        string[] row = tsv.Split('\n');
        int rowSize = row.Length;
        int colSize = row[0].Split('\t').Length;

        // row��ŭ �ݺ���
        for (int i = 0; i < rowSize; i++)
        {

            string[] col = row[i].Split('\t');
            descList.Add(new List<string>());
            for (int j = 0; j < colSize; j++)
            {
                descList[i].Add(col[j]);
            }

            descDic.Add(int.Parse(descList[i][(int)DescColData.ID]), descList[i]);
        }
    }

    #endregion

    #region Tower

    private const string TowerURL = "https://docs.google.com/spreadsheets/d/1IkitwusiwPWK0fK9i1gbCqsgtLl1YQBJ/export?format=tsv&gid=625995306&range=A5:AL124";

    [Header("[타워]")]
    public TowerDatabaseList towerDatabaseListCSV;
    public ItemDataBaseList towerDatabaseList;
    public ItemDataBaseList tower_Attack_DatabaseList;
    public ItemDataBaseList tower_Buff_DatabaseList;
    public ItemDataBaseList tower_Minion_DatabaseList;

    UnityWebRequest TowerWebData;
    public Dictionary<int, TowerBlueprint> TowerDic = new Dictionary<int, TowerBlueprint>();
    IEnumerator GetTowerData()
    {
        TowerWebData = UnityWebRequest.Get(TowerURL);
        yield return TowerWebData.SendWebRequest();

        string DB = TowerWebData.downloadHandler.text;
        SetTowerData(DB);
    }


    public void SetTowerData(string tsv)
    {
        string[] row = tsv.Split('\n');
        int rowSize = row.Length;
        int columnSize = row[0].Split('\t').Length;

        for (int i = 0; i < rowSize; i++)
        {
            string[] col = row[i].Split('\t');

            // 기본정보
            towerDatabaseListCSV.itemList[i].ID = int.Parse(col[0]);
            towerDatabaseListCSV.itemList[i].Pf = Resources.Load<GameObject>(col[3]);
            towerDatabaseListCSV.itemList[i].NickName = col[5];
            towerDatabaseListCSV.itemList[i].Name = col[4];

            // 조합
            towerDatabaseListCSV.itemList[i].Combination_ResultID = int.Parse(col[6]);
            towerDatabaseListCSV.itemList[i].Combination_Required_Value = int.Parse(col[7]);

            // 뽑기
            towerDatabaseListCSV.itemList[i].Normal_Random_Draw_Probability = float.Parse(col[8]);
            towerDatabaseListCSV.itemList[i].Normal_Attack_Draw_Probability = float.Parse(col[9]);
            towerDatabaseListCSV.itemList[i].Normal_Minion_Draw_Probability = float.Parse(col[10]);
            towerDatabaseListCSV.itemList[i].Normal_Buff_Debuff_Draw_Probability = float.Parse(col[11]);

            towerDatabaseListCSV.itemList[i].Premium_Random_Draw_Probability = float.Parse(col[12]);
            towerDatabaseListCSV.itemList[i].Premium_Attack_Draw_Probability = float.Parse(col[13]);
            towerDatabaseListCSV.itemList[i].Premium_Minion_Draw_Probability = float.Parse(col[14]);
            towerDatabaseListCSV.itemList[i].Premium_Buff_Debuff_Draw_Probability = float.Parse(col[15]);

            // 속성
            towerDatabaseListCSV.itemList[i].GroupID = int.Parse(col[16]);
            towerDatabaseListCSV.itemList[i].Rank = int.Parse(col[17]);
            towerDatabaseListCSV.itemList[i].Type = int.Parse(col[18]);
            towerDatabaseListCSV.itemList[i].Attack = float.Parse(col[19]);
            towerDatabaseListCSV.itemList[i].Attack_Speed = float.Parse(col[20]);
            towerDatabaseListCSV.itemList[i].Hp = int.Parse(col[21]);
            towerDatabaseListCSV.itemList[i].Range = int.Parse(col[22]);
            towerDatabaseListCSV.itemList[i].Range_Type = int.Parse(col[23]);

            // 투사체
            towerDatabaseListCSV.itemList[i].Projectile_Speed = float.Parse(col[24]);
            towerDatabaseListCSV.itemList[i].Projectile_Type = int.Parse(col[25]);
            towerDatabaseListCSV.itemList[i].Projectile_Pf = Resources.Load<GameObject>(col[26]);

            if (int.Parse(col[33]) != 0)
            {
                towerDatabaseListCSV.itemList[i].Desc = descDic[int.Parse(col[33])][(int)DescColData.Text_En];
            }

            // 부가옵션
            towerDatabaseListCSV.itemList[i].Destroy_Effect_Pf = Resources.Load<GameObject>(col[30]);
            towerDatabaseListCSV.itemList[i].Sprite_TowerCard = Resources.Load<Sprite>("Sprites/TowerImage/" + col[34]);
            towerDatabaseListCSV.itemList[i].Sprite_TowerProtrait = Resources.Load<Sprite>("Sprites/TowerIcon/" + col[35]);
            towerDatabaseListCSV.itemList[i].AudioClip_Attack = Resources.Load<AudioClip>("Sounds/" + col[36]);
            towerDatabaseListCSV.itemList[i].AudioClip_Destroy = Resources.Load<AudioClip>("Sounds/ES_Break");

            // 버프타워만 해당
            towerDatabaseListCSV.itemList[i].buffID = int.Parse(col[31]);

            // 미니언타워만 해당.
            towerDatabaseListCSV.itemList[i].MinionID = int.Parse(col[32]);

            // 다른 스크립트에서 조회할수 있게 딕셔너리에 담기.
            TowerDic.Add(towerDatabaseListCSV.itemList[i].ID, towerDatabaseListCSV.itemList[i]);

            towerDatabaseList.itemList[i + 1].towerData = towerDatabaseListCSV.itemList[i];
            towerDatabaseList.itemList[i + 1].itemIcon = towerDatabaseListCSV.itemList[i].Sprite_TowerCard;
            towerDatabaseList.itemList[i + 1].itemDesc = towerDatabaseListCSV.itemList[i].Desc;


            if (i < 50)
            {
                tower_Attack_DatabaseList.itemList[i + 1].towerData = towerDatabaseListCSV.itemList[i];
                tower_Attack_DatabaseList.itemList[i + 1].itemIcon = towerDatabaseListCSV.itemList[i].Sprite_TowerCard;
                tower_Attack_DatabaseList.itemList[i + 1].itemDesc = towerDatabaseListCSV.itemList[i].Desc;
            }

            else if (i >= 50 && i < 90)
            {
                tower_Buff_DatabaseList.itemList[i - 49].towerData = towerDatabaseListCSV.itemList[i];
                tower_Buff_DatabaseList.itemList[i - 49].itemIcon = towerDatabaseListCSV.itemList[i].Sprite_TowerCard;
                tower_Buff_DatabaseList.itemList[i - 49].itemDesc = towerDatabaseListCSV.itemList[i].Desc;
            }

            else
            {
                tower_Minion_DatabaseList.itemList[i - 89].towerData = towerDatabaseListCSV.itemList[i];
                tower_Minion_DatabaseList.itemList[i - 89].itemIcon = towerDatabaseListCSV.itemList[i].Sprite_TowerCard;
                tower_Minion_DatabaseList.itemList[i - 89].itemDesc = towerDatabaseListCSV.itemList[i].Desc;
            }
        }
    }

    #endregion Tower

    #region Buff

    private const string BuffURL = "https://docs.google.com/spreadsheets/d/1IkitwusiwPWK0fK9i1gbCqsgtLl1YQBJ/export?format=tsv&gid=1296679834&range=A4:K86";

    [Header("[버프]")]
    public BuffDatabaseList buffDatabaseListCSV;
    public Dictionary<int, BuffBlueprint> BuffDic = new Dictionary<int, BuffBlueprint>();

    UnityWebRequest BuffWebData;
    IEnumerator GetBuffData()
    {
        BuffWebData = UnityWebRequest.Get(BuffURL);
        yield return BuffWebData.SendWebRequest();

        string DB = BuffWebData.downloadHandler.text;
        SetBuffData(DB);
    }

    public void SetBuffData(string tsv)
    {
        string[] row = tsv.Split('\n');
        int rowSize = row.Length;
        int columnSize = row[0].Split('\t').Length;

        for (int i = 0; i < rowSize; i++)
        {
            string[] col = row[i].Split('\t');

            // 기본정보
            buffDatabaseListCSV.itemList[i].ID = int.Parse(col[0]);
            buffDatabaseListCSV.itemList[i].Name = col[2] + col[4];
            buffDatabaseListCSV.itemList[i].Icon = Resources.Load<Sprite>("Sprites/BuffIcon/" + col[2]);
            buffDatabaseListCSV.itemList[i].GroupID = int.Parse(col[3]);
            buffDatabaseListCSV.itemList[i].Rank = int.Parse(col[4]);
            buffDatabaseListCSV.itemList[i].Type = int.Parse(col[5]);
            buffDatabaseListCSV.itemList[i].Target = int.Parse(col[6]);
            buffDatabaseListCSV.itemList[i].Value = float.Parse(col[7]);
            buffDatabaseListCSV.itemList[i].Duration = float.Parse(col[8]);
            buffDatabaseListCSV.itemList[i].Desc = descDic[int.Parse(col[9])][(int)DescColData.Text_En];
            buffDatabaseListCSV.itemList[i].ToolTipName = col[10];

            BuffDic.Add(buffDatabaseListCSV.itemList[i].ID, buffDatabaseListCSV.itemList[i]);
        }
    }
    #endregion Buff 

    #region Minion

    private const string MinionURL = "https://docs.google.com/spreadsheets/d/1IkitwusiwPWK0fK9i1gbCqsgtLl1YQBJ/export?format=tsv&gid=653157190&range=A5:R59";

    [Header("[미니언]")]
    public MinionDatabaseList MinionDatabaseListCSV;

    UnityWebRequest MinionWebData;
    public Dictionary<int, MinionBlueprint> MinionDic = new Dictionary<int, MinionBlueprint>();
    IEnumerator GetMinionData()
    {
        MinionWebData = UnityWebRequest.Get(MinionURL);
        yield return MinionWebData.SendWebRequest();

        string DB = MinionWebData.downloadHandler.text;
        SetMinionData(DB);
    }

    public void SetMinionData(string tsv)
    {
        string[] row = tsv.Split('\n');
        int rowSize = row.Length;
        int columnSize = row[0].Split('\t').Length;

        for (int i = 0; i < rowSize; i++)
        {
            string[] col = row[i].Split('\t');

            // 기본정보
            MinionDatabaseListCSV.itemList[i].ID = int.Parse(col[0]);
            MinionDatabaseListCSV.itemList[i].Name = col[3] + col[5];
            MinionDatabaseListCSV.itemList[i].GroupID = int.Parse(col[4]);
            MinionDatabaseListCSV.itemList[i].Rank = int.Parse(col[5]);
            MinionDatabaseListCSV.itemList[i].Type = int.Parse(col[6]);

            // 투사체 정보
            MinionDatabaseListCSV.itemList[i].Projectile_Speed = float.Parse(col[9]);
            MinionDatabaseListCSV.itemList[i].Projectile_Type = int.Parse(col[10]);

            // 속성
            MinionDatabaseListCSV.itemList[i].Attack = float.Parse(col[11]);
            MinionDatabaseListCSV.itemList[i].Attack_Speed = float.Parse(col[12]);
            MinionDatabaseListCSV.itemList[i].Range = float.Parse(col[13]);
            MinionDatabaseListCSV.itemList[i].Move_Speed = float.Parse(col[14]);
            MinionDatabaseListCSV.itemList[i].Hp = float.Parse(col[15]);
            MinionDatabaseListCSV.itemList[i].Exp = float.Parse(col[16]);
            MinionDatabaseListCSV.itemList[i].Icon_Blue = Resources.Load<Sprite>("Sprites/MinionIcon/" + col[3] + "_Blue");
            MinionDatabaseListCSV.itemList[i].Icon_Red = Resources.Load<Sprite>("Sprites/MinionIcon/" + col[3] + "_Red");

            MinionDic.Add(MinionDatabaseListCSV.itemList[i].ID, MinionDatabaseListCSV.itemList[i]);
        }
    }
    #endregion Minion 

    #region SkillDataParsing

    private const string wizardSkillURL = "https://docs.google.com/spreadsheets/d/1PnBV0AFMfz3PdaEXZJcOPjnQCCQCOGoV/export?format=tsv&range=A4:Y18";
    private const string warriorSkillURL = "https://docs.google.com/spreadsheets/d/1ggp4p3CU3bRVbeF-Eq6UshL67FK0VHwV/export?format=tsv&range=A4:Y18";
    private const string commonSkillURL = "https://docs.google.com/spreadsheets/d/1cJp9QbYYOzvVO9TVZUfe9XsWLgEMnlc3/export?format=tsv&range=A4:Y12";

    Dictionary<int, List<string>> WarriorSkillDatas = new Dictionary<int, List<string>>();
    List<List<string>> WarriorSkillRowDatas = new List<List<string>>();

    Dictionary<int, List<string>> WizardSkillDatas = new Dictionary<int, List<string>>();
    List<List<string>> WizardSkillRowDatas = new List<List<string>>();

    Dictionary<int, List<string>> CommonSkillDatas = new Dictionary<int, List<string>>();
    List<List<string>> CommonSkillRowDatas = new List<List<string>>();

    [Header("[스킬]")]
    public SkillDatas WarriorSkillParsing;
    public SkillDatas WizardSkillParsing;
    public SkillDatas CommonSkillParsing;
    public ItemDataBaseList WarriorDatabaseList;
    public ItemDataBaseList WizardDatabaseList;
    public ItemDataBaseList CommonDatabaseList;

    IEnumerator GetWizardSkillData(string url)
    {
        UnityWebRequest WizardSkillDataRequest = UnityWebRequest.Get(url);
        yield return WizardSkillDataRequest.SendWebRequest();
        SplitSkillDatas(WizardSkillDataRequest.downloadHandler.text, WizardSkillDatas, WizardSkillRowDatas, WizardSkillParsing, WizardDatabaseList);

    }

    IEnumerator GetWarriorSkillData(string url)
    {
        UnityWebRequest WarriorSkillDataRequest = UnityWebRequest.Get(url);
        yield return WarriorSkillDataRequest.SendWebRequest();
        SplitSkillDatas(WarriorSkillDataRequest.downloadHandler.text, WarriorSkillDatas, WarriorSkillRowDatas, WarriorSkillParsing, WarriorDatabaseList);

    }

    IEnumerator GetCommonSkillData(string url)
    {
        UnityWebRequest CommonSkillDataRequest = UnityWebRequest.Get(url);
        yield return CommonSkillDataRequest.SendWebRequest();
        SplitSkillDatas(CommonSkillDataRequest.downloadHandler.text, CommonSkillDatas, CommonSkillRowDatas, CommonSkillParsing, CommonDatabaseList);
        onDataParsingEvent.Invoke();
        Debug.Log("파싱 완료.. Invoke중");
    }

    private void SplitSkillDatas(string tsv, Dictionary<int, List<string>> dic, List<List<string>> list, SkillDatas job, ItemDataBaseList oldData)
    {
        string[] row = tsv.Split('\n');
        int rowSize = row.Length;
        int colSize = row[0].Split('\t').Length;

        for (int i = 0; i < rowSize; i++)
        {
            list.Add(new List<string>());
            string[] col = row[i].Split('\t');
            for (int j = 0; j < colSize; j++)
            {
                list[i].Add(col[j]);
            }
            dic.Add(i + 1, list[i]);
        }
        CharactorSkillDataInput(rowSize, job, dic, oldData);
    }

    public void CharactorSkillDataInput(int size, SkillDatas skillDatas, Dictionary<int, List<string>> dic, ItemDataBaseList oldData)
    {
        for (int i = 0; i < size; i++)
        {
            skillDatas.DataList[i].ID = int.Parse(dic[i + 1][(int)SkillColData.ID]);
            skillDatas.DataList[i].Name = Resources.Load<GameObject>(dic[i + 1][(int)SkillColData.Name]);
            skillDatas.DataList[i].NameLevel = dic[i + 1][(int)SkillColData.NameLevel];
            skillDatas.DataList[i].Probability = float.Parse(dic[i + 1][(int)SkillColData.Probability]);
            skillDatas.DataList[i].Classification = int.Parse(dic[i + 1][(int)SkillColData.Classification]);
            skillDatas.DataList[i].Rank = int.Parse(dic[i + 1][(int)SkillColData.Rank]);
            skillDatas.DataList[i].LockTime = float.Parse(dic[i + 1][(int)SkillColData.LockTime]);

            skillDatas.DataList[i].SkillType = int.Parse(dic[i + 1][(int)SkillColData.SkillType]);
            skillDatas.DataList[i].Value_1 = float.Parse(dic[i + 1][(int)SkillColData.Value1]);
            skillDatas.DataList[i].Value_2 = float.Parse(dic[i + 1][(int)SkillColData.Value2]);

            skillDatas.DataList[i].CoolTime = float.Parse(dic[i + 1][(int)SkillColData.CoolTime]);

            skillDatas.DataList[i].Range = float.Parse(dic[i + 1][(int)SkillColData.Range]);
            skillDatas.DataList[i].RangeValue_1 = float.Parse(dic[i + 1][(int)SkillColData.RangeValue1]);
            skillDatas.DataList[i].RangeValue_2 = float.Parse(dic[i + 1][(int)SkillColData.RangeValue2]);

            skillDatas.DataList[i].HoldingTime = float.Parse(dic[i + 1][(int)SkillColData.HoldingTime]);

            skillDatas.DataList[i].TickDamage = float.Parse(dic[i + 1][(int)SkillColData.TickDamage]);
            skillDatas.DataList[i].TickCount = int.Parse(dic[i + 1][(int)SkillColData.TickCount]);
            skillDatas.DataList[i].TickTime = int.Parse(dic[i + 1][(int)SkillColData.TickTime]);

            skillDatas.DataList[i].CcType = int.Parse(dic[i + 1][(int)SkillColData.CcType]);
            skillDatas.DataList[i].CcTime = float.Parse(dic[i + 1][(int)SkillColData.CcTime]);
            skillDatas.DataList[i].CcValue = float.Parse(dic[i + 1][(int)SkillColData.CcValue]);

            skillDatas.DataList[i].CardImage = Resources.Load<Sprite>("Sprites/SKillImage/" + dic[i + 1][(int)SkillColData.CardImage]);
            skillDatas.DataList[i].SkillIcon = Resources.Load<Sprite>("Sprites/SkillIcon/" + dic[i + 1][(int)SkillColData.SkillIcon]);
            skillDatas.DataList[i].Desc = descDic[int.Parse(dic[i + 1][(int)SkillColData.Desc])][(int)DescColData.Text_En];

            oldData.itemList[i + 1].skillData = skillDatas.DataList[i];
            oldData.itemList[i + 1].itemName = skillDatas.DataList[i].NameLevel;
            oldData.itemList[i + 1].itemIcon = skillDatas.DataList[i].CardImage;
            oldData.itemList[i + 1].itemDesc = skillDatas.DataList[i].Desc;
        }


    }

    #endregion

    #region Charactor
    private const string warriorStatURL = "https://docs.google.com/spreadsheets/d/1cMpFR0mXGTAqoFxhMgu32bFFhkJuvxmy/export?format=tsv&gid=511513918&range=A4:K10";
    private const string wizardStatURL = "https://docs.google.com/spreadsheets/d/1cMpFR0mXGTAqoFxhMgu32bFFhkJuvxmy/export?format=tsv&gid=511513918&range=A11:K17";

    Dictionary<int, List<string>> warriorStatDatas = new Dictionary<int, List<string>>();
    List<List<string>> warriorStatRowDatas = new List<List<string>>();

    Dictionary<int, List<string>> wizardStatDatas = new Dictionary<int, List<string>>();
    List<List<string>> wizardStatRowDatas = new List<List<string>>();

    [Header("[스탯]")]
    public StatDatas warriorStatParsing;
    public StatDatas wizardStatParsing;

    IEnumerator GetWizardStatData(string url)
    {
        UnityWebRequest wizardStatDataRequest = UnityWebRequest.Get(url);
        yield return wizardStatDataRequest.SendWebRequest();
        SplitStatDatas(wizardStatDataRequest.downloadHandler.text, wizardStatDatas, wizardStatRowDatas, wizardStatParsing);
    }

    IEnumerator GetWarriorStatData(string url)
    {
        UnityWebRequest warriorStatDataRequest = UnityWebRequest.Get(url);
        yield return warriorStatDataRequest.SendWebRequest();
        SplitStatDatas(warriorStatDataRequest.downloadHandler.text, warriorStatDatas, warriorStatRowDatas, warriorStatParsing);
    }

    private void SplitStatDatas(string tsv, Dictionary<int, List<string>> dic, List<List<string>> list, StatDatas job)
    {
        string[] row = tsv.Split('\n');
        int rowSize = row.Length;
        int colSize = row[0].Split('\t').Length;


        for (int i = 0; i < rowSize; i++)
        {
            list.Add(new List<string>());
            string[] col = row[i].Split('\t');
            for (int j = 0; j < colSize; j++)
            {
                list[i].Add(col[j]);
            }
            dic.Add(i + 1, list[i]);
        }

        CharactorStatDataInput(rowSize, dic, job);
    }

    private void CharactorStatDataInput(int size, Dictionary<int, List<string>> dic, StatDatas statDatas)
    {
        for (int i = 0; i < size; i++)
        {
            statDatas.dataList[i].id = int.Parse(dic[i + 1][(int)StatColData.ID]);
            statDatas.dataList[i].name = dic[i + 1][(int)StatColData.Name];
            statDatas.dataList[i].nameLevel = dic[i + 1][(int)StatColData.NameLevel];
            statDatas.dataList[i].hp = float.Parse(dic[i + 1][(int)StatColData.HP]);
            statDatas.dataList[i].damage = float.Parse(dic[i + 1][(int)StatColData.Dmg]);
            statDatas.dataList[i].range = float.Parse(dic[i + 1][(int)StatColData.Range]);
            statDatas.dataList[i].attackSpeed = float.Parse(dic[i + 1][(int)StatColData.Atk_Speed]);
            statDatas.dataList[i].moveSpeed = float.Parse(dic[i + 1][(int)StatColData.Move_Speed]);
            statDatas.dataList[i].maxExp = int.Parse(dic[i + 1][(int)StatColData.Max_Exp]);
            statDatas.dataList[i].charID = int.Parse(dic[i + 1][(int)StatColData.Character_ID]);
            statDatas.dataList[i].expEnemy = int.Parse(dic[i + 1][(int)StatColData.Exp_Enemy]);
        }
    }
    #endregion

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

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


public class CSVtest : MonoBehaviour
{
    #region DescParsing

    private const string descURL = "https://docs.google.com/spreadsheets/d/1ta3EbfGEC9NswgOeCqHaI25BO9sPvpc2/export?format=tsv&range=A3:D44";

    // Ű������ ID, List���� Name KoTooltip, EnTooltip�� ����
    Dictionary<int, List<string>> descDic = new Dictionary<int, List<string>>();
    List<List<string>> descList = new List<List<string>>();

    bool isDone = false;

    IEnumerator GetDescData(string url)
    {
        UnityWebRequest DescDataRequest = UnityWebRequest.Get(url);
        yield return DescDataRequest.SendWebRequest();
        SplitDescData(DescDataRequest.downloadHandler.text);
        Debug.Log("Desc�Ľ̿Ϸ�");
        StartCoroutine(GetLevelData());
        StartCoroutine(GetWarriorSkillData(warriorSkillURL));
        StartCoroutine(GetWizardSkillData(wizardSkillURL));
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
            // ����Ʈ�� ù��°�� ��Ҵ� ID��
            descDic.Add(int.Parse(descList[i][(int)DescColData.ID]), descList[i]);
        }
    }

    #endregion



    private const string URL = "https://docs.google.com/spreadsheets/d/1FOm8D4Hb0IbgmNOnSLiLrV7HpSgB-kjS/export?format=tsv&gid=625995306&range=A5:AM124";

    private string MYtext = "";

    [Header("[Ÿ��]")]
    public TowerDatabaseList towerDatabaseListCSV;
    public ItemDataBaseList towerDatabaseList;
    public ItemDataBaseList tower_Attack_DatabaseList;
    public ItemDataBaseList tower_Buff_DatabaseList;
    public ItemDataBaseList tower_Minion_DatabaseList;

    private void Start()
    {
        StartCoroutine(GetDescData(descURL));
    }

    UnityWebRequest GetCharactorData;
    float elaspedTime = 0f;
    IEnumerator GetLevelData()
    {
        GetCharactorData = UnityWebRequest.Get(URL);
        Debug.Log(elaspedTime);
        yield return GetCharactorData.SendWebRequest();
        Debug.Log(elaspedTime);

        MYtext = GetCharactorData.downloadHandler.text;
        Debug.Log(MYtext);
        SetCharactorDatas(MYtext);
        Debug.Log("tower�Ľ̿Ϸ�");

    }

    public void SetCharactorDatas(string tsv)
    {
        string[] row = tsv.Split('\n');
        int rowSize = row.Length;
        int columnSize = row[0].Split('\t').Length;

        for (int i = 0; i < rowSize; i++)
        {
            string[] col = row[i].Split('\t');

            // �⺻����
            towerDatabaseListCSV.itemList[i].ID = int.Parse(col[0]);
            towerDatabaseListCSV.itemList[i].Pf = Resources.Load<GameObject>(col[3]);
            towerDatabaseListCSV.itemList[i].Name = col[4];

            // ����
            towerDatabaseListCSV.itemList[i].Combination_ResultID = int.Parse(col[6]);
            towerDatabaseListCSV.itemList[i].Combination_Required_Value = int.Parse(col[7]);

            // �̱�
            towerDatabaseListCSV.itemList[i].Normal_Random_Draw_Probability = float.Parse(col[8]);
            towerDatabaseListCSV.itemList[i].Normal_Attack_Draw_Probability = float.Parse(col[9]);
            towerDatabaseListCSV.itemList[i].Normal_Minion_Draw_Probability = float.Parse(col[10]);
            towerDatabaseListCSV.itemList[i].Normal_Buff_Debuff_Draw_Probability = float.Parse(col[11]);

            towerDatabaseListCSV.itemList[i].Premium_Random_Draw_Probability = float.Parse(col[12]);
            towerDatabaseListCSV.itemList[i].Premium_Attack_Draw_Probability = float.Parse(col[13]);
            towerDatabaseListCSV.itemList[i].Premium_Minion_Draw_Probability = float.Parse(col[14]);
            towerDatabaseListCSV.itemList[i].Premium_Buff_Debuff_Draw_Probability = float.Parse(col[15]);

            // �Ӽ�
            towerDatabaseListCSV.itemList[i].GroupID = int.Parse(col[16]);
            towerDatabaseListCSV.itemList[i].Rank = int.Parse(col[17]);
            towerDatabaseListCSV.itemList[i].Type = int.Parse(col[18]);
            towerDatabaseListCSV.itemList[i].Attack = float.Parse(col[19]);
            towerDatabaseListCSV.itemList[i].Attack_Speed = float.Parse(col[20]);
            towerDatabaseListCSV.itemList[i].Hp = int.Parse(col[21]);
            towerDatabaseListCSV.itemList[i].Range = int.Parse(col[22]);
            towerDatabaseListCSV.itemList[i].Range_Type = int.Parse(col[23]);

            // ����ü
            towerDatabaseListCSV.itemList[i].Projectile_Speed = float.Parse(col[24]);
            towerDatabaseListCSV.itemList[i].Projectile_Type = int.Parse(col[25]);
            towerDatabaseListCSV.itemList[i].Projectile_Pf = Resources.Load<GameObject>(col[26]);

            // �ΰ��ɼ�
            towerDatabaseListCSV.itemList[i].Destroy_Effect_Pf_Name = col[30];
            towerDatabaseListCSV.itemList[i].Desc = descDic[int.Parse(col[33])][(int)DescColData.Text_Ko];
            towerDatabaseListCSV.itemList[i].Sprite_TowerCard = Resources.Load<Sprite>("Sprites/TowerImage/" + col[34]);

            towerDatabaseListCSV.itemList[i].Sprite_TowerProtrait = Resources.Load<Sprite>("Sprites/TowerIcon/" + col[35]);
            towerDatabaseListCSV.itemList[i].AudioClip_Attack_Name = col[36];
            towerDatabaseListCSV.itemList[i].AudioClip_Hit_Name = col[37];
            towerDatabaseListCSV.itemList[i].AudioClip_Normal_Name = col[38];

            towerDatabaseList.itemList[i + 1].towerData = towerDatabaseListCSV.itemList[i];
            towerDatabaseList.itemList[i + 1].itemModel = towerDatabaseListCSV.itemList[i].Pf;
            towerDatabaseList.itemList[i + 1].itemIcon = towerDatabaseListCSV.itemList[i].Sprite_TowerCard;

            if (i < 50)
            {
                tower_Attack_DatabaseList.itemList[i + 1].towerData = towerDatabaseListCSV.itemList[i];
                tower_Attack_DatabaseList.itemList[i + 1].itemModel = towerDatabaseListCSV.itemList[i].Pf;
                tower_Attack_DatabaseList.itemList[i + 1].itemIcon = towerDatabaseListCSV.itemList[i].Sprite_TowerCard;
            }

            else if (i >= 50 && i < 90)
            {
                tower_Buff_DatabaseList.itemList[i - 49].towerData = towerDatabaseListCSV.itemList[i];
                tower_Buff_DatabaseList.itemList[i - 49].itemModel = towerDatabaseListCSV.itemList[i].Pf;
                tower_Buff_DatabaseList.itemList[i - 49].itemIcon = towerDatabaseListCSV.itemList[i].Sprite_TowerCard;
            }

            else
            {
                tower_Minion_DatabaseList.itemList[i - 89].towerData = towerDatabaseListCSV.itemList[i];
                tower_Minion_DatabaseList.itemList[i - 89].itemModel = towerDatabaseListCSV.itemList[i].Pf;
                tower_Minion_DatabaseList.itemList[i - 89].itemIcon = towerDatabaseListCSV.itemList[i].Sprite_TowerCard;
            }
        }
    }










    #region SkillDataParsing

    private const string wizardSkillURL = "https://docs.google.com/spreadsheets/d/1PnBV0AFMfz3PdaEXZJcOPjnQCCQCOGoV/export?format=tsv&range=A4:Y18";
    private const string warriorSkillURL = "https://docs.google.com/spreadsheets/d/1ggp4p3CU3bRVbeF-Eq6UshL67FK0VHwV/export?format=tsv&range=A4:Y18";

    Dictionary<int, List<string>> WarriorSkillDatas = new Dictionary<int, List<string>>();
    List<List<string>> WarriorSkillRowDatas = new List<List<string>>();

    Dictionary<int, List<string>> WizardSkillDatas = new Dictionary<int, List<string>>();
    List<List<string>> WizardSkillRowDatas = new List<List<string>>();

    [Header("[��ų]")]
    public SkillDatas WarriorSkillParsing;
    public SkillDatas WizardSkillParsing;
    public ItemDataBaseList WarriorDatabaseList;
    public ItemDataBaseList WizardDatabaseList;

    IEnumerator GetWizardSkillData(string url)
    {
        UnityWebRequest WizardSkillDataRequest = UnityWebRequest.Get(url);
        yield return WizardSkillDataRequest.SendWebRequest();
        SplitSkillDatas(WizardSkillDataRequest.downloadHandler.text, WizardSkillDatas, WizardSkillRowDatas, WizardSkillParsing, WizardDatabaseList);
        Debug.Log("skill�Ľ̿Ϸ�");

    }

    IEnumerator GetWarriorSkillData(string url)
    {
        UnityWebRequest WarriorSkillDataRequest = UnityWebRequest.Get(url);
        yield return WarriorSkillDataRequest.SendWebRequest();
        SplitSkillDatas(WarriorSkillDataRequest.downloadHandler.text, WarriorSkillDatas, WarriorSkillRowDatas, WarriorSkillParsing, WarriorDatabaseList);
        Debug.Log("skill�Ľ̿Ϸ�");

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
        CharactorDataInput(rowSize, job, dic, oldData);
    }

    public void CharactorDataInput(int size, SkillDatas skillDatas, Dictionary<int, List<string>> dic, ItemDataBaseList oldData)
    {
        for (int i = 0; i < size; i++)
        {
            skillDatas.DataList[i].ID = int.Parse(dic[i + 1][(int)SkillColData.ID]);
            skillDatas.DataList[i].Name = Resources.Load<GameObject>("Skills/" + dic[i + 1][(int)SkillColData.Name]);
            skillDatas.DataList[i].NameLevel = dic[i + 1][(int)SkillColData.NameLevel];
            skillDatas.DataList[i].Probability = float.Parse(dic[i + 1][(int)SkillColData.Probability]);
            skillDatas.DataList[i].Classification = int.Parse(dic[i + 1][(int)SkillColData.Classification]);
            skillDatas.DataList[i].Rank = int.Parse(dic[i + 1][(int)SkillColData.Rank]);
            skillDatas.DataList[i].LockTime = float.Parse(dic[i + 1][(int)SkillColData.LockTime]);

            skillDatas.DataList[i].SkillType = int.Parse(dic[i + 1][(int)SkillColData.SkillType]);
            skillDatas.DataList[i].Value_1 = int.Parse(dic[i + 1][(int)SkillColData.Value1]);
            skillDatas.DataList[i].Value_2 = int.Parse(dic[i + 1][(int)SkillColData.Value2]);

            skillDatas.DataList[i].CoolTime = float.Parse(dic[i + 1][(int)SkillColData.CoolTime]);

            skillDatas.DataList[i].Range = float.Parse(dic[i + 1][(int)SkillColData.Range]);
            skillDatas.DataList[i].RangeValue_1 = int.Parse(dic[i + 1][(int)SkillColData.RangeValue1]);
            skillDatas.DataList[i].RangeValue_2 = int.Parse(dic[i + 1][(int)SkillColData.RangeValue2]);

            skillDatas.DataList[i].HoldingTime = float.Parse(dic[i + 1][(int)SkillColData.HoldingTime]);

            skillDatas.DataList[i].TickDamage = float.Parse(dic[i + 1][(int)SkillColData.TickDamage]);
            skillDatas.DataList[i].TickCount = int.Parse(dic[i + 1][(int)SkillColData.TickCount]);
            skillDatas.DataList[i].TickTime = int.Parse(dic[i + 1][(int)SkillColData.TickTime]);

            skillDatas.DataList[i].CcType = int.Parse(dic[i + 1][(int)SkillColData.CcType]);
            skillDatas.DataList[i].CcTime = float.Parse(dic[i + 1][(int)SkillColData.CcTime]);
            skillDatas.DataList[i].CcValue = float.Parse(dic[i + 1][(int)SkillColData.CcValue]);

            skillDatas.DataList[i].CardImage = Resources.Load<Sprite>("Sprites/SKillImage/" + dic[i + 1][(int)SkillColData.CardImage]);
            skillDatas.DataList[i].SkillIcon = Resources.Load<Sprite>("Sprites/SkillIcon/" + dic[i + 1][(int)SkillColData.SkillIcon]);

            //skillDatas.DataList[i].Desc = dic[i + 1][(int)SkillColData.Desc];
            skillDatas.DataList[i].Desc = descDic[int.Parse(dic[i + 1][(int)SkillColData.Desc])][(int)DescColData.Text_En];

            // ������ ���� �����ͺ��̽�����Ʈ�� ����(�ϴ���)
            oldData.itemList[i + 1].skillData = skillDatas.DataList[i];
            oldData.itemList[i + 1].itemName = skillDatas.DataList[i].NameLevel;
            oldData.itemList[i + 1].itemIcon = skillDatas.DataList[i].CardImage;
        }
    }

    #endregion
}
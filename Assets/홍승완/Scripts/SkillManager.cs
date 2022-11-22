using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum ColData
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


public class SkillManager : MonoBehaviour
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public static SkillManager Instance;
    public TrojanHorse SkillData;

    public GameObject[] SkillPF = new GameObject[4];
    public float[] Atk = new float[4];
    public float[] CoolTime = new float[4];
    public float[] HoldingTime = new float[4];
    public float[] Range = new float[4];
    public float[] LockTime = new float[4];

    #region SkillDataParsing

    private const string wizardSkillURL = "https://docs.google.com/spreadsheets/d/1PnBV0AFMfz3PdaEXZJcOPjnQCCQCOGoV/export?format=tsv&range=A4:Y18";
    private const string warriorSkillURL = "https://docs.google.com/spreadsheets/d/1ggp4p3CU3bRVbeF-Eq6UshL67FK0VHwV/export?format=tsv&range=A4:Y18";

    Dictionary<int, List<string>> CharactorSkillDatas = new Dictionary<int, List<string>>();

    List<List<string>> RowDatas = new List<List<string>>();

    public List<PlayerSkillDatas> AllSkillDatas;
    //public List<PlayerSkillDatas> AllWizardSkillDatas;
    //public List<PlayerSkillDatas> AllWarriorSkillDatas;

    IEnumerator GetSkillData(string url)
    {
        UnityWebRequest getSkillData = UnityWebRequest.Get(url);
        yield return getSkillData.SendWebRequest();
        SplitSkillDatas(getSkillData.downloadHandler.text);
    }

    public void SplitSkillDatas(string tsv)
    {
        Debug.Log($"tsv : {tsv}");

        RowDatas.Clear();
        CharactorSkillDatas.Clear();

        string[] row = tsv.Split('\n');
        int rowSize = row.Length;
        int colSize = row[0].Split('\t').Length;

        for (int i = 0; i < rowSize; i++)
        {
            RowDatas.Add(new List<string>());
            string[] col = row[i].Split('\t');
            for (int j = 0; j < colSize; j++)
            {
                RowDatas[i].Add(col[j]);
            }

            CharactorSkillDatas.Add(i + 1, RowDatas[i]);
        }

        WizardDataInput(rowSize);
    }

    public void WizardDataInput(int size)
    {
        for (int i = 0; i < size; i++)
        {
            AllSkillDatas.Add(new PlayerSkillDatas());
        }

        #region tsv사용
        for (int i = 0; i < size; i++)
        {
            AllSkillDatas[i].ID = int.Parse(CharactorSkillDatas[i + 1][(int)ColData.ID]);
            AllSkillDatas[i].Name = CharactorSkillDatas[i + 1][(int)ColData.Name];
            AllSkillDatas[i].NameLevel = CharactorSkillDatas[i + 1][(int)ColData.NameLevel];
            AllSkillDatas[i].Probability = float.Parse(CharactorSkillDatas[i + 1][(int)ColData.Probability]);
            AllSkillDatas[i].Classification = int.Parse(CharactorSkillDatas[i + 1][(int)ColData.Classification]);
            AllSkillDatas[i].Rank = int.Parse(CharactorSkillDatas[i + 1][(int)ColData.Rank]);
            AllSkillDatas[i].LockTime = float.Parse(CharactorSkillDatas[i + 1][(int)ColData.LockTime]);

            AllSkillDatas[i].SkillType = int.Parse(CharactorSkillDatas[i + 1][(int)ColData.SkillType]);
            AllSkillDatas[i].Value_1 = int.Parse(CharactorSkillDatas[i + 1][(int)ColData.Value1]);
            AllSkillDatas[i].Value_2 = int.Parse(CharactorSkillDatas[i + 1][(int)ColData.Value2]);

            AllSkillDatas[i].CoolTime = float.Parse(CharactorSkillDatas[i + 1][(int)ColData.CoolTime]);

            AllSkillDatas[i].Range = float.Parse(CharactorSkillDatas[i + 1][(int)ColData.Range]);
            AllSkillDatas[i].RangeValue_1 = int.Parse(CharactorSkillDatas[i + 1][(int)ColData.RangeValue1]);
            AllSkillDatas[i].RangeValue_2 = int.Parse(CharactorSkillDatas[i + 1][(int)ColData.RangeValue2]);

            AllSkillDatas[i].HoldingTime = float.Parse(CharactorSkillDatas[i + 1][(int)ColData.HoldingTime]);

            AllSkillDatas[i].TickDamage = float.Parse(CharactorSkillDatas[i + 1][(int)ColData.TickDamage]);
            AllSkillDatas[i].TickCount = int.Parse(CharactorSkillDatas[i + 1][(int)ColData.TickCount]);
            AllSkillDatas[i].TickTime = int.Parse(CharactorSkillDatas[i + 1][(int)ColData.TickTime]);

            AllSkillDatas[i].CcType = int.Parse(CharactorSkillDatas[i + 1][(int)ColData.CcType]);
            AllSkillDatas[i].CcTime = float.Parse(CharactorSkillDatas[i + 1][(int)ColData.CcTime]);
            AllSkillDatas[i].CcValue = float.Parse(CharactorSkillDatas[i + 1][(int)ColData.CcValue]);

            AllSkillDatas[i].Desc = CharactorSkillDatas[i + 1][(int)ColData.Desc];
        }

        #endregion
    }

    #endregion

    private void Awake()
    {
        Instance = this;
        SkillData = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>();

        //StartCoroutine(GetSkillData(wizardSkillURL));
        //StartCoroutine(GetSkillData(warriorSkillURL));
    }

    private void Start()
    {
        #region 기존방식
        int count = SkillData.skillIndex.Count;
        if (count == 0)
        {
            return;
        }
        //int idx = _skillManager.SkillData.skillIndex[i];
        for (int i = 0; i < count; i++)
        {
            int idx = SkillData.skillIndex[i];

            SkillPF[idx] = SkillData.skillItems[i].itemModel;
            Atk[idx] = SkillData.skillItems[i].itemAttributes[0].attributeValue;
            CoolTime[idx] = SkillData.skillItems[i].itemAttributes[1].attributeValue;
            HoldingTime[idx] = SkillData.skillItems[i].itemAttributes[2].attributeValue;
            Range[idx] = SkillData.skillItems[i].itemAttributes[3].attributeValue;
            LockTime[idx] = SkillData.skillItems[i].itemAttributes[4].attributeValue;
        }

        #endregion

 

    }

    private void OnDisable()
    {
        Instance = null;
    }

}

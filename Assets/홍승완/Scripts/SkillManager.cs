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

    Dictionary<int, List<string>> WizardSkillDatas = new Dictionary<int, List<string>>();
    List<List<string>> RowDatas = new List<List<string>>();

    public List<PlayerSkillDatas> AllWizardSkillDatas;

    IEnumerator GetSkillData(string url)
    {
        UnityWebRequest getSkillData = UnityWebRequest.Get(url);
        yield return getSkillData.SendWebRequest();
        SplitSkillDatas(getSkillData.downloadHandler.text);
    }

    public void SplitSkillDatas(string tsv)
    {
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
            WizardSkillDatas.Add(i + 1, RowDatas[i]);
        }

        DataInput(rowSize);
        Debug.Log("데이터 파싱완료");
    }

    public void DataInput(int size)
    {
        #region tsv사용
        Debug.Log($"size? : {size}");
        for (int i = 0; i < size; i++)
        {
            AllWizardSkillDatas[i].ID = int.Parse(WizardSkillDatas[i + 1][(int)ColData.ID]);
            AllWizardSkillDatas[i].Name = WizardSkillDatas[i + 1][(int)ColData.Name];
            AllWizardSkillDatas[i].NameLevel = WizardSkillDatas[i + 1][(int)ColData.NameLevel];
            AllWizardSkillDatas[i].Probability = float.Parse(WizardSkillDatas[i + 1][(int)ColData.Probability]);
            AllWizardSkillDatas[i].Classification = int.Parse(WizardSkillDatas[i + 1][(int)ColData.Classification]);
            AllWizardSkillDatas[i].Rank = int.Parse(WizardSkillDatas[i + 1][(int)ColData.Rank]);
            AllWizardSkillDatas[i].LockTime = float.Parse(WizardSkillDatas[i + 1][(int)ColData.LockTime]);

            AllWizardSkillDatas[i].SkillType = int.Parse(WizardSkillDatas[i + 1][(int)ColData.SkillType]);
            AllWizardSkillDatas[i].Value_1 = int.Parse(WizardSkillDatas[i + 1][(int)ColData.Value1]);
            AllWizardSkillDatas[i].Value_2 = int.Parse(WizardSkillDatas[i + 1][(int)ColData.Value2]);

            AllWizardSkillDatas[i].CoolTime = float.Parse(WizardSkillDatas[i + 1][(int)ColData.CoolTime]);

            AllWizardSkillDatas[i].Range = float.Parse(WizardSkillDatas[i + 1][(int)ColData.Range]);
            AllWizardSkillDatas[i].RangeValue_1 = int.Parse(WizardSkillDatas[i + 1][(int)ColData.RangeValue1]);
            AllWizardSkillDatas[i].RangeValue_2 = int.Parse(WizardSkillDatas[i + 1][(int)ColData.RangeValue2]);

            AllWizardSkillDatas[i].HoldingTime = float.Parse(WizardSkillDatas[i + 1][(int)ColData.HoldingTime]);

            AllWizardSkillDatas[i].TickDamage = float.Parse(WizardSkillDatas[i + 1][(int)ColData.TickDamage]);
            AllWizardSkillDatas[i].TickCount = int.Parse(WizardSkillDatas[i + 1][(int)ColData.TickCount]);
            AllWizardSkillDatas[i].TickTime = int.Parse(WizardSkillDatas[i + 1][(int)ColData.TickTime]);

            AllWizardSkillDatas[i].CcType = int.Parse(WizardSkillDatas[i + 1][(int)ColData.CcType]);
            AllWizardSkillDatas[i].CcTime = float.Parse(WizardSkillDatas[i + 1][(int)ColData.CcTime]);
            AllWizardSkillDatas[i].CcValue = float.Parse(WizardSkillDatas[i + 1][(int)ColData.CcValue]);

            AllWizardSkillDatas[i].Desc = WizardSkillDatas[i + 1][(int)ColData.Desc];
        }

        #endregion
    }

    #endregion

    private void Awake()
    {
        StartCoroutine(GetSkillData(wizardSkillURL));

        Instance = this;
        SkillData = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public static SkillManager Instance;
    //[SerializeField]
    //ItemDataBaseList ItemDataBase;
    public TrojanHorse SkillData;

    public GameObject[] SkillPF = new GameObject[4];
    public float[] Atk = new float[4];
    public float[] CoolTime = new float[4];
    public float[] HoldingTime = new float[4];
    public float[] Range = new float[4];

    private void Awake()
    {
        Instance = this;
        SkillData = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>();
        Debug.Log("스킬매니저 awake실행");
    }

    private void Start()
    {
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

            Debug.Log($"{SkillData.skillItems[i].itemAttributes[0].attributeName}의 값 : {Atk[idx]}\n" +
                $"{SkillData.skillItems[i].itemAttributes[1].attributeName}의 값 : {CoolTime[idx]}\n" +
                $"{SkillData.skillItems[i].itemAttributes[2].attributeName}의 값 : {HoldingTime[idx]}\n" +
                $"{SkillData.skillItems[i].itemAttributes[3].attributeName}의 값 : {Range[idx]}\n");
        }
    }
    
}

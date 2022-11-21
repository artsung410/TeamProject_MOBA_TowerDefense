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
    public float[] LockTime = new float[4];

    private void Awake()
    {
        Instance = this;
        SkillData = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>();
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
            LockTime[idx] = SkillData.skillItems[i].itemAttributes[4].attributeValue;
        }
    }


}

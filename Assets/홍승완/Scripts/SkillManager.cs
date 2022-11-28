#define PARSING_VER
//#define OLD_VER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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


#if OLD_VER
    private void Awake()
    {
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

#endif



#if PARSING_VER
    public PlayerSkillDatas[] Datas = new PlayerSkillDatas[4];

    private void Awake()
    {
        Instance = this;
        SkillData = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>();

        if (SkillData.skillIndex.Count == 0)
        {
            return;
        }

        for (int i = 0; i < SkillData.skillIndex.Count; i++)
        {
            int idx = SkillData.skillIndex[i];

            Datas[idx] = SkillData.skillItems[i].skillData;
        }
    }

#endif

    private void OnDisable()
    {
        Instance = null;
    }
}

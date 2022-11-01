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
    [SerializeField]
    ItemDataBaseList ItemDataBase;
    TrojanHorse aa;

    public float[] Atk = new float[4];
    public float[] CoolTime = new float[4];
    public float[] HoldingTime = new float[4];
    public float[] Range = new float[4];

    private void Awake()
    {
        Instance = this;
        aa = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>();

    }

    private void Start()
    {
        int count = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>().skillItems.Count;

        if (count == 0)
        {
            return;
        }

        else
        {
            for (int i = 0; i < count; i++) // 4
            {
                for (int j = 0; j < ItemDataBase.itemList.Count; j++) // 100
                {
                    if (aa.skillId[i] == ItemDataBase.itemList[j].itemID)
                    {
                        Atk[i] = ItemDataBase.itemList[j].itemAttributes[0].attributeValue;
                        CoolTime[i] = ItemDataBase.itemList[j].itemAttributes[1].attributeValue;
                        HoldingTime[i] = ItemDataBase.itemList[j].itemAttributes[2].attributeValue;
                        Range[i] = ItemDataBase.itemList[j].itemAttributes[3].attributeValue;
                    }
                }
            }
        

        }
    }
    
}

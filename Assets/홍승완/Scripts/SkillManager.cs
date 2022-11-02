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
    TrojanHorse horse;

    public float[] Atk = new float[4];
    public float[] CoolTime = new float[4];
    public float[] HoldingTime = new float[4];
    public float[] Range = new float[4];

    private void Awake()
    {
        Instance = this;
        horse = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>();

    }

    private void Start()
    {
        int count = horse.skillIndex.Count;
        if (count == 0)
        {
            return;
        }

        for (int i = 0; i < count; i++)
        {
            int idx = horse.skillIndex[i];
            for (int j = 0; j < ItemDataBase.itemList.Count; j++)
            {
                if (horse.skillId[i] == ItemDataBase.itemList[j].itemID)
                {
                    Atk[idx] = ItemDataBase.itemList[j].itemAttributes[0].attributeValue;
                    CoolTime[idx] = ItemDataBase.itemList[j].itemAttributes[1].attributeValue;
                    HoldingTime[idx] = ItemDataBase.itemList[j].itemAttributes[2].attributeValue;
                    Range[idx] = ItemDataBase.itemList[j].itemAttributes[3].attributeValue;
                }
            }
        }
    }
    
}

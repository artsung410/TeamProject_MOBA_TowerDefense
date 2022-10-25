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

    public float[] atk = new float[4];
    public float[] time = new float[4];

    private void Awake()
    {
        Instance = this;
        aa = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>();

    }

    private void Update()
    {
        

        for (int i = 0; i < 4; i++) // 4
        {
            for (int j = 0; j < ItemDataBase.itemList.Count; j++) // 100
            {
                if (aa.skillId[i] == ItemDataBase.itemList[j].itemID)
                {
                    atk[i] = ItemDataBase.itemList[j].itemAttributes[0].attributeValue;
                    time[i] = ItemDataBase.itemList[j].itemAttributes[1].attributeValue;
                }
            }
        }
    }
}

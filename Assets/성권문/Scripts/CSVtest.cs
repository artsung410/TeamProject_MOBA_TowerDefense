using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class CSVtest : MonoBehaviour
{
    private const string URL = "https://docs.google.com/spreadsheets/d/1FOm8D4Hb0IbgmNOnSLiLrV7HpSgB-kjS/export?format=tsv&gid=625995306&range=A5:AM124";
                               
    private string MYtext = "";

    [Header("[타워]")]
    public TowerDatabaseList towerDatabaseList_Ingame;
    public ItemDataBaseList towerDatabaseList_Robby;

    private void Start()
    {
        StartCoroutine(GetLevelData());
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
    }

    public void SetCharactorDatas(string tsv)
    {
        string[] row = tsv.Split('\n');
        int rowSize = row.Length;
        int columnSize = row[0].Split('\t').Length;

        for (int i = 0; i < rowSize; i++)
        {
            string[] col = row[i].Split('\t');   

            // 기본정보
            towerDatabaseList_Ingame.itemList[i].ID = int.Parse(col[0]);
            towerDatabaseList_Ingame.itemList[i].Pf = Resources.Load<GameObject>(col[3]);
            towerDatabaseList_Ingame.itemList[i].Name = col[4];

            // 조합
            towerDatabaseList_Ingame.itemList[i].Combination_ResultID = int.Parse(col[6]);
            towerDatabaseList_Ingame.itemList[i].Combination_Required_Value = int.Parse(col[7]);

            // 뽑기
            towerDatabaseList_Ingame.itemList[i].Normal_Random_Draw_Probability = float.Parse(col[8]);
            towerDatabaseList_Ingame.itemList[i].Normal_Attack_Draw_Probability = float.Parse(col[9]);
            towerDatabaseList_Ingame.itemList[i].Normal_Minion_Draw_Probability = float.Parse(col[10]);
            towerDatabaseList_Ingame.itemList[i].Normal_Buff_Debuff_Draw_Probability = float.Parse(col[11]);

            towerDatabaseList_Ingame.itemList[i].Premium_Random_Draw_Probability = float.Parse(col[12]);
            towerDatabaseList_Ingame.itemList[i].Premium_Attack_Draw_Probability = float.Parse(col[13]);
            towerDatabaseList_Ingame.itemList[i].Premium_Minion_Draw_Probability = float.Parse(col[14]);
            towerDatabaseList_Ingame.itemList[i].Premium_Buff_Debuff_Draw_Probability = float.Parse(col[15]);

            // 속성
            towerDatabaseList_Ingame.itemList[i].GroupID = int.Parse(col[16]);
            towerDatabaseList_Ingame.itemList[i].Rank = int.Parse(col[17]);
            towerDatabaseList_Ingame.itemList[i].Type = int.Parse(col[18]);
            towerDatabaseList_Ingame.itemList[i].Attack = float.Parse(col[19]);
            towerDatabaseList_Ingame.itemList[i].Attack_Speed = float.Parse(col[20]);
            towerDatabaseList_Ingame.itemList[i].Hp = int.Parse(col[21]);
            towerDatabaseList_Ingame.itemList[i].Range = int.Parse(col[22]);
            towerDatabaseList_Ingame.itemList[i].Range_Type = int.Parse(col[23]);

            // 투사체
            towerDatabaseList_Ingame.itemList[i].Projectile_Speed = float.Parse(col[24]);
            towerDatabaseList_Ingame.itemList[i].Projectile_Type = int.Parse(col[25]);
            towerDatabaseList_Ingame.itemList[i].Projectile_Pf = Resources.Load<GameObject>(col[26]);

            // 부가옵션
            towerDatabaseList_Ingame.itemList[i].Destroy_Effect_Pf_Name = col[30];
            towerDatabaseList_Ingame.itemList[i].Desc = col[33];
            towerDatabaseList_Ingame.itemList[i].Sprite_TowerCard = Resources.Load<Sprite>("Sprites/" + col[34]);
            towerDatabaseList_Ingame.itemList[i].Sprite_TowerProtrait = Resources.Load<Sprite>("Sprites/TowerPortrait" + col[35]);

            towerDatabaseList_Ingame.itemList[i].AudioClip_Attack_Name = col[36];
            towerDatabaseList_Ingame.itemList[i].AudioClip_Hit_Name = col[37];
            towerDatabaseList_Ingame.itemList[i].AudioClip_Normal_Name = col[38];

        }

        Debug.Log($"int rowSize : {rowSize}");
        Debug.Log($"columnSize : {columnSize}");
    }
}
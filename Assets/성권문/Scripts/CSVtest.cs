using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class CSVtest : MonoBehaviour
{
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
            towerDatabaseListCSV.itemList[i].Desc = col[33];
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
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class CSVtest : MonoBehaviour
{
    private const string URL = "https://docs.google.com/spreadsheets/d/1Qlp1QXfwrp5AdR5SE1El6H0OL8pm3vq_/edit#gid=625995306/export?format=tsv&range=A5:AL124";
    private string MYtext = "";

    public List<Ghost> ghosts;

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

        //MYtext = GetCharactorData.downloadHandler.text;
        //Debug.Log(MYtext);
        //SetCharactorDatas(MYtext);
    }

    private void Update()
    {
        elaspedTime += Time.deltaTime;
        if(GetCharactorData.isDone)
        {  
            Debug.Log($"{elaspedTime}");
        }
    }

    //public void SetCharactorDatas(string tsv)
    //{
    //    string[] row = tsv.Split('\n');
    //    int rowSize = row.Length;
    //    int columnSize = row[0].Split('\t').Length;

    //    for (int i = 0; i < rowSize; i++)
    //    {
    //        string[] col = row[i].Split('\t');   // row  = 고양이,강아지,병아리
    //        // col = [고양이, 강아지, 병아리]
    //        ghosts[i].id = int.Parse(col[0]);
    //        ghosts[i].hp = int.Parse(col[1]);
    //        ghosts[i].strength = int.Parse(col[2]);
    //        ghosts[i].xpReward = int.Parse(col[3]);
    //        Debug.Log($"monster{ghosts[i].id} info // hp :{ghosts[i].hp}, strength : {ghosts[i].strength}, xpReward : {ghosts[i].xpReward}");
    //    }

    //    Debug.Log($"int rowSize : {rowSize}");
    //    Debug.Log($"columnSize : {columnSize}");
    //}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Networking;



public class GoogleSheetManager : MonoBehaviourPun
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    // export?format=tsv => tsv형식으로 데이터 파싱
    // &range=A2:I => 영역 설정 A2부터 I전체
    // 시트를 추가할 경우 주소창 gid 숫자가 바뀐다 => gid숫자로 시트판단 가능
        // format=tsv&gid=1843253394 : 두번째 시트를 파싱하기위해 gid추가
    protected const string WarriorURL = "https://docs.google.com/spreadsheets/d/160L4eKjHOegKnuPtxl1tYHeAgdv-jX00_OoZqnG8t_A/export?format=tsv&range=B2:H";

    protected Dictionary<int, List<string>> WarriorLevelData = new Dictionary<int, List<string>>();

    protected List<List<string>> levelDatas = new List<List<string>>();

    private void Awake()
    {
        //StartCoroutine(GetLevelData());
    }

    IEnumerator GetLevelData()
    {
        UnityWebRequest requestWarriorData = UnityWebRequest.Get(WarriorURL);
        yield return requestWarriorData.SendWebRequest();
        SetWarriorStats(requestWarriorData.downloadHandler.text);
    }

    public void SetWarriorStats(string tsv)
    {
        string[] row = tsv.Split('\n');
        int rowSize = row.Length;
        int columnSize = row[0].Split('\t').Length;

        for (int i = 0; i < rowSize; i++)
        {
            levelDatas.Add(new List<string>());

            string[] column = row[i].Split('\t');

            for (int j = 0; j < columnSize; j++)
            {
                levelDatas[i].Add(column[j]);
            }

            WarriorLevelData.Add(i + 1, levelDatas[i]);
        }

    }


}

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

    // export?format=tsv => tsv�������� ������ �Ľ�
    // &range=A2:I => ���� ���� A2���� I��ü
    // ��Ʈ�� �߰��� ��� �ּ�â gid ���ڰ� �ٲ�� => gid���ڷ� ��Ʈ�Ǵ� ����
        // format=tsv&gid=1843253394 : �ι�° ��Ʈ�� �Ľ��ϱ����� gid�߰�
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

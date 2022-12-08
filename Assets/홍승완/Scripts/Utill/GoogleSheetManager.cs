using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Networking;



public abstract class GoogleSheetManager : MonoBehaviourPun
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    // export?format=tsv => tsv�������� ������ �Ľ�
    // &range=A2:I => ���� ���� A2���� I��ü
    // ��Ʈ�� �߰��� ��� �ּ�â gid ���ڰ� �ٲ�� => gid���ڷ� ��Ʈ�Ǵ� ����
        // format=tsv&gid=1843253394 : �ι�° ��Ʈ�� �Ľ��ϱ����� gid�߰�

    #region ���۽��������ƮDB

    protected const string warriorURL = "https://docs.google.com/spreadsheets/d/1cMpFR0mXGTAqoFxhMgu32bFFhkJuvxmy/exfort?format=tsv&gid=511513918&range=D4:K10";

    protected const string magicionURL = "https://docs.google.com/spreadsheets/d/1cMpFR0mXGTAqoFxhMgu32bFFhkJuvxmy/exfort?format=tsv&gid=511513918&range=D11:K17";

    #endregion

    protected Dictionary<int, List<string>> CharactorLevelData = new Dictionary<int, List<string>>();

    protected List<List<string>> levelDatas = new List<List<string>>();


    public abstract void SetCharactorDatas(string tsv);


}

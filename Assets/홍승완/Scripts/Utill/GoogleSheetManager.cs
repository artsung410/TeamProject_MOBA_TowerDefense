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

    // export?format=tsv => tsv형식으로 데이터 파싱
    // &range=A2:I => 영역 설정 A2부터 I전체
    // 시트를 추가할 경우 주소창 gid 숫자가 바뀐다 => gid숫자로 시트판단 가능
        // format=tsv&gid=1843253394 : 두번째 시트를 파싱하기위해 gid추가

    #region 구글스프레드시트DB

    protected const string warriorURL = "https://docs.google.com/spreadsheets/d/1Ia6nhRo4KaXRp4dxQl92a12QB_t6tnVw/export?format=tsv&gid=96320651&range=D4:K10";

    protected const string magicionURL = "https://docs.google.com/spreadsheets/d/1Ia6nhRo4KaXRp4dxQl92a12QB_t6tnVw/export?format=tsv&gid=1889391686&range=D4:K10";

    #endregion

    protected Dictionary<int, List<string>> CharactorLevelData = new Dictionary<int, List<string>>();

    protected List<List<string>> levelDatas = new List<List<string>>();


    public abstract void SetCharactorDatas(string tsv);


}

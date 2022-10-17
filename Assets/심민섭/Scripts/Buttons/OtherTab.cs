using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 버튼을 누르면 기타텝이 열린다.
public class OtherTab : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    private bool openOtherTab = false;

    public void OpenOtherTab()
    {
        openOtherTab = true;
        gameObject.SetActive(true);

        // 다른 텝은 다 닫기
        if (openOtherTab)
        {
            gameObject.transform.parent.parent.GetChild(1).GetChild(0).gameObject.SetActive(false);
            gameObject.transform.parent.parent.GetChild(1).GetChild(1).gameObject.SetActive(false);
            openOtherTab = false;
        }
    }
}

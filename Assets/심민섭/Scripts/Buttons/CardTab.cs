using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 버튼을 누르면 카드텝이 열린다.
public class CardTab : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    private bool openCardTab = false;

    public void OpenCardTab()
    {
        openCardTab = true;
        gameObject.SetActive(true);
        gameObject.transform.parent.GetChild(0).gameObject.SetActive(true);
        // 다른 텝은 다 닫기
        if (openCardTab)
        {
            gameObject.transform.parent.parent.GetChild(2).GetChild(0).gameObject.SetActive(false);
            openCardTab = false;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��ư�� ������ ��Ÿ���� ������.
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

        // �ٸ� ���� �� �ݱ�
        if (openOtherTab)
        {
            gameObject.transform.parent.parent.GetChild(1).GetChild(0).gameObject.SetActive(false);
            gameObject.transform.parent.parent.GetChild(1).GetChild(1).gameObject.SetActive(false);
            openOtherTab = false;
        }
    }
}

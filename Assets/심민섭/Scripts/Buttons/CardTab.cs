using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��ư�� ������ ī������ ������.
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
        // �ٸ� ���� �� �ݱ�
        if (openCardTab)
        {
            gameObject.transform.parent.parent.GetChild(2).GetChild(0).gameObject.SetActive(false);
            openCardTab = false;
        }
    }

}

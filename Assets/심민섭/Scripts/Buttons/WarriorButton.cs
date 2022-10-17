using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 버튼을 누르면 워리어 탭창이 열린다.
public class WarriorButton : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    private bool openWarriorTab;

    public void OpenWarriorTab()
    {
        openWarriorTab = !openWarriorTab;
        gameObject.SetActive(openWarriorTab);

        // 다른 텝은 다 닫기
        if (openWarriorTab)
        {
            for (int i = 1; i < 4; i++)
            {
                gameObject.transform.parent.parent.GetChild(i).GetChild(1).gameObject.SetActive(false);
            }
            //"WizardTab", "InherenceTab", "AssassinTab"); "WarriorTab"
        }
    }
}

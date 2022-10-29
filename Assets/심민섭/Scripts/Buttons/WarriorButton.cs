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
        openWarriorTab = true;
        gameObject.SetActive(openWarriorTab);

        // 다른 텝은 다 닫기
        if (openWarriorTab)
        {
            for (int i = 0; i < 4; i++)
            {
                if (i == 0)
                {
                    continue;
                }
                gameObject.transform.parent.parent.GetChild(i).GetChild(0).gameObject.SetActive(false);
            }
            //"WizardTab", "InherenceTab", "AssassinTab"); "WarriorTab"
        }
    }
}

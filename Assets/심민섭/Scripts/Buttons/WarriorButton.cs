using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��ư�� ������ ������ ��â�� ������.
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

        // �ٸ� ���� �� �ݱ�
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

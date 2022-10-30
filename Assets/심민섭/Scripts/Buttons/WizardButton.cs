using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardButton : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    private bool openWizardTab;

    public void OpenWizardTab()
    {
        openWizardTab = true;
        gameObject.SetActive(openWizardTab);

        // ¥Ÿ∏• ≈‹¿∫ ¥Ÿ ¥›±‚
        if (openWizardTab)
        {
            for (int i = 0; i < 4; i++)
            {
                if (i == 1)
                {
                    continue;
                }
                gameObject.transform.parent.parent.GetChild(i).GetChild(0).gameObject.SetActive(false);
            }
        }
    }
}

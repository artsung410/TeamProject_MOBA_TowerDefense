using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinButton : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    private bool openAssassinTab;

    public void OpenAssassTab()
    {
        openAssassinTab = true;
        gameObject.SetActive(openAssassinTab);

        // ¥Ÿ∏• ≈‹¿∫ ¥Ÿ ¥›±‚
        if (openAssassinTab)
        {
            for (int i = 0; i < 4; i++)
            {
                if (i == 2)
                {
                    continue;
                }
                gameObject.transform.parent.parent.GetChild(i).GetChild(0).gameObject.SetActive(false);
            }
        }
    }
}

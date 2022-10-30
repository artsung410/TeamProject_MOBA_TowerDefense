using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InherenceButton : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    private bool openInherenceTab;

    public void OpenInherenceTab()
    {
        openInherenceTab = true;
        gameObject.SetActive(openInherenceTab);

        // ¥Ÿ∏• ≈‹¿∫ ¥Ÿ ¥›±‚
        if (openInherenceTab)
        {
            for (int i = 0; i < 4; i++)
            {
                if (i == 3)
                {
                    continue;
                }
                gameObject.transform.parent.parent.GetChild(i).GetChild(0).gameObject.SetActive(false);
            }
        }
    }
}

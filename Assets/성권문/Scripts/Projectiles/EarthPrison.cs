using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com         
// ###############################################

public class EarthPrison : Projectiles
{
    private void Start()
    {
        if (gameObject == null)
        {
            return;
        }

        Destroy(gameObject, 3f);
    }
}

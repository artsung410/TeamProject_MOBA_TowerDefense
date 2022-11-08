using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    
    // ###############################################
    //             NAME : ARTSUNG                      
    //             MAIL : artsung410@gmail.com         
    // ###############################################

public class CircleAttack : Projectiles
{
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject == null)
        {
            return;
        }

        Destroy(gameObject, 3f);
    }
}

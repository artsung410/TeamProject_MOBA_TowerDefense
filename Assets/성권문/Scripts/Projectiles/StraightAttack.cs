using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class StraightAttack : Projectiles
{
    // ###############################################
    //             NAME : ARTSUNG                      
    //             MAIL : artsung410@gmail.com         
    // ###############################################

    void OnParticleCollision(GameObject other)
    {
        Damage(other.transform);
        Debug.Log(other.transform.name + "��ƼŬ �浹�١١١١١١١١١١١١١١١١١١١١١١١١١١١١١١١١١١١١١١١١١١١١١١١١١١١١١١١١١١١١١١١١١١١١�");
    }
}

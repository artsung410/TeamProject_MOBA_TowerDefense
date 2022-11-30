using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    //HeroCombat combat;
    [SerializeField] PlayerBehaviour _playerScript;
    [SerializeField] Health _playerHP;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == _playerScript.EnemyTag || other.gameObject.layer == 17)
        {
            if (_playerScript != null)
            {
                if (_playerScript.IsAttack)
                {
                    _playerScript.enemyCol = other;
                    Debug.Log($"Ä®¿¡ ´êÀº °´Ã¼ : {other}");
                }
            }
            else
            {
                return;
            }
        }
    }

}

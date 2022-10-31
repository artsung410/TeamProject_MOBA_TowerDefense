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
    [SerializeField] Stats _stat;


    private void OnTriggerEnter(Collider other)
    {
        // �ִϸ��̼� �̺�Ʈ ������ ����
        //_playerScript.enemyCol = null;

        if (other == null)
        {
            return;
        }
        if (other.tag == _playerScript.EnemyTag)
        {
            if (_playerScript != null)
            {
                if (_playerScript.IsAttack)
                {
                    _playerScript.enemyCol = other;

                }
            }
            else
            {
                return;
            }
        }
    }
}

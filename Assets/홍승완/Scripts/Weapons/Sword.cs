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

    private void Awake()
    {
        //_playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("¡¢√À");
        // enemyø° ¥Íæ“¡ˆ∏∏ targeted∞° null¿œ∂ß ≥Œø¿∑˘ ≥≤
        
        if (other.tag == _playerScript.EnemyTag && _playerScript.perfomMeleeAttack)
        {
            if (_playerScript != null)
            {
                //_playerScript.MeleeAttack();
                _playerHP.OnDamage(_stat.attackDmg, other);
                //Debug.Log("¡¢√À«‘");
            }
            else
            {
                //Debug.Log("no ¡¢√À");
                return;
            }
        }

    }
}

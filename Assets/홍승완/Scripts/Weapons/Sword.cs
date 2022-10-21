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
        //Debug.Log("접촉");
        // enemy에 닿았지만 targeted가 null일때 널오류 남
        
        if (other.tag == _playerScript.EnemyTag)
        {
            if (_playerScript != null)
            {
                    Debug.Log($"공격중? : {_playerScript.IsAttack}");
                //_playerScript.MeleeAttack();
                if (_playerScript.IsAttack)
                {
                    //_playerHP.OnDamage(_stat.attackDmg, other);
                    other.GetComponent<Health>().OnDamage(_stat.attackDmg);
                }
                //Debug.Log("접촉함");
            }
            else
            {
                //Debug.Log("no 접촉");
                return;
            }
        }

    }
}

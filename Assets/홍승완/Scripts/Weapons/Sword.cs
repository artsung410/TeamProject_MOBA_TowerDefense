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

    private void Awake()
    {
        //_playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //// enemy에 닿았지만 targeted가 null일때 널오류 남
        //if (other.CompareTag("Enemy"))
        //{
        //    if (_playerScript != null)
        //    {
        //        _playerScript.MeleeAttack();
        //        Debug.Log("접촉함");
        //    }
        //    else
        //    {
        //        Debug.Log("no 접촉");
        //        return;
        //    }
        //}
        _playerScript.MeleeAttack();
    }
}

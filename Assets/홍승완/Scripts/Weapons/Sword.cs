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
        //// enemy�� ������� targeted�� null�϶� �ο��� ��
        //if (other.CompareTag("Enemy"))
        //{
        //    if (_playerScript != null)
        //    {
        //        _playerScript.MeleeAttack();
        //        Debug.Log("������");
        //    }
        //    else
        //    {
        //        Debug.Log("no ����");
        //        return;
        //    }
        //}
        _playerScript.MeleeAttack();
    }
}

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
    Collider enemyCol;

    private void Awake()
    {
        //_playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _playerScript.enemyCol = null;
        //enemyCol = null;
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
                    //enemyCol = other;
                    _playerScript.enemyCol = other;

                    //if (enemyCol.gameObject.layer == 7)
                    //{
                    //    enemyCol.GetComponent<Health>().OnDamage(_stat.attackDmg);
                    //}
                    //else if (enemyCol.gameObject.layer == 8 || enemyCol.gameObject.layer == 13)
                    //{
                    //    enemyCol.GetComponent<Enemybase>().TakeDamage(_stat.attackDmg);
                    //}
                    //else if (enemyCol.gameObject.layer == 6)
                    //{
                    //    enemyCol.GetComponent<Turret>().Damage(_stat.attackDmg);
                    //}
                    //else if (enemyCol.gameObject.layer == 12)
                    //{
                    //    NexusHp temp = enemyCol.GetComponent<NexusHp>();
                    //    enemyCol.GetComponent<NexusHp>().TakeOnDagmage(_stat.attackDmg);
                    //}

                }
            }
            else
            {
                return;
            }
        }
    }
}

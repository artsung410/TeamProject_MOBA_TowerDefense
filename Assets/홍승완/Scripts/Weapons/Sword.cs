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
                    //_playerHP.OnDamage(_stat.attackDmg, other);
                    if (other.gameObject.layer == 7)
                    {
                        other.GetComponent<Health>().OnDamage(_stat.attackDmg);
                    }
                    else if (other.gameObject.layer == 8 || other.gameObject.layer == 13)
                    {
                        other.GetComponent<Enemybase>().TakeDamage(_stat.attackDmg);
                    }
                    else if (other.gameObject.layer == 6)
                    {
                        other.GetComponent<Turret>().TakeDamage(_stat.attackDmg);
                    }
                    else if (other.gameObject.layer == 12)
                    {
                        NexusHp temp = other.GetComponent<NexusHp>();
                        other.GetComponent<NexusHp>().TakeOnDagmage(_stat.attackDmg);
                    }
                    
                }
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

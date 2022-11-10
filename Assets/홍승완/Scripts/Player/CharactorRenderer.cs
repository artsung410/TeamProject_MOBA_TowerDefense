using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CharactorRenderer : MonoBehaviourPun
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public GameObject ColPosition;
    PlayerBehaviour  _behaviour;
    Stats _stat;

    Vector3 interpolation = new Vector3(0, 2, 0);

    private void Awake()
    {
        _behaviour = ColPosition.GetComponent<PlayerBehaviour>();
        _stat = ColPosition.GetComponent<Stats>();
    }
    private void LateUpdate()
    {
        if (photonView.IsMine)
        {
            transform.position = ColPosition.transform.position - interpolation;
            transform.rotation = ColPosition.transform.localRotation;
        }
    }


    public void SwordSwingAtTheEnemy()
    {
        if (photonView.IsMine)
        {
            if (_behaviour.enemyCol == null)
            {
                return;
            }

            if (_behaviour.enemyCol.gameObject.layer == 7)
            {
                _behaviour.enemyCol.GetComponent<Health>().OnDamage(_stat.attackDmg);
                Debug.Log("확인");
            }
            else if (_behaviour.enemyCol.gameObject.layer == 8 || _behaviour.enemyCol.gameObject.layer == 13)
            {
                _behaviour.enemyCol.GetComponent<Enemybase>().TakeDamage(_stat.attackDmg);
                Debug.Log("확인");
            }
            else if (_behaviour.enemyCol.gameObject.layer == 6)
            {
                _behaviour.enemyCol.GetComponent<Turret>().Damage(_stat.attackDmg);
                Debug.Log("확인");
            }
            else if (_behaviour.enemyCol.gameObject.layer == 12)
            {
                _behaviour.enemyCol.GetComponent<NexusHp>().TakeOnDagmage(_stat.attackDmg);
                Debug.Log("확인");
            }
        }
    }
}

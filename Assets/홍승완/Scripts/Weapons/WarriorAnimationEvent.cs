using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WarriorAnimationEvent : MonoBehaviourPun
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public PlayerBehaviour _behaviour;
    public Stats _stat;
    public BoxCollider swordCol;


    #region �ִϸ��̼� �̺�Ʈ �Լ�
    public void OnSwordCol()
    {
        swordCol.enabled = true;
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
                //Debug.Log($"���� Ÿ�� :{_behaviour.enemyCol.gameObject.name}");
            }
            else if (_behaviour.enemyCol.gameObject.layer == 8 || _behaviour.enemyCol.gameObject.layer == 13 || _behaviour.enemyCol.gameObject.layer == 17)
            {
                if(_behaviour.enemyCol.GetComponent<Enemybase>()._eminontpye == EMINIONTYPE.Netural)
                {
                _behaviour.enemyCol.GetComponent<Enemybase>().tagThrow(_stat.gameObject.tag);
                }
                _behaviour.enemyCol.GetComponent<Enemybase>().TakeDamage(_stat.attackDmg);
                
                //Debug.Log($"���� Ÿ�� :{_behaviour.enemyCol.gameObject.name}");

            }
            else if (_behaviour.enemyCol.gameObject.layer == 6)
            {
                _behaviour.enemyCol.GetComponent<Turret>().Damage(_stat.attackDmg);
                //Debug.Log($"���� Ÿ�� :{_behaviour.enemyCol.gameObject.name}");

            }
            else if (_behaviour.enemyCol.gameObject.layer == 12)
            {
                if (_behaviour.enemyCol.GetComponent<NexusHp>() == null)
                {
                    return;
                }
                // TODO : 2Ÿ���� null���� ����Ը��ؾ���
                _behaviour.enemyCol.GetComponent<NexusHp>().TakeOnDagmage(_stat.attackDmg);                
            }

            swordCol.enabled = false;
        }

    }

    #endregion
    

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class NetualAttack : MonoBehaviourPun
{
    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################
    [SerializeField]
    private OrcFSM _orcFSM;
    [SerializeField]
    private float Knockforce;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (other.CompareTag("Blue") || other.CompareTag("Red"))
            {
                if (other.gameObject.layer == 7)
                { 
                    other.gameObject.GetComponent<Health>().OnDamage(_orcFSM.Damage);
                    
                }
                else if (other.gameObject.layer == 8 || other.gameObject.layer == 13)
                {
                    
                    other.gameObject.GetComponent<Enemybase>().TakeDamage(_orcFSM.Damage);
                }

            }
        }
    }

    private void KnockBack()
    {

    }
}

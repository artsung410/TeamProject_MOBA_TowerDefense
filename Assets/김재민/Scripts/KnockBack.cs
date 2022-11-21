using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class KnockBack : MonoBehaviourPun
{
    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################
    [SerializeField]
    private float Knockforce;

    void Start()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (PhotonNetwork.IsMasterClient)
        {

            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 direction = collision.transform.position - transform.position;
                direction.y = 0;
                rb.AddForce(direction.normalized * Knockforce, ForceMode.Impulse);
            }
        }
    }
}

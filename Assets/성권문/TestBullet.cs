using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TestBullet : MonoBehaviourPun
{
    float Damage = 25f;
    float moveSpeed = 10f;
    private void FixedUpdate()
    {
        if(!photonView.IsMine)
        {
            return;
        }

        transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (other.gameObject.CompareTag("Blue") || other.gameObject.CompareTag("Red"))
       {
            Debug.Log("Å¸¿ö Á¢ÃËÇÔ");
            other.gameObject.GetComponent<Turret>().Damage(Damage);
            PhotonNetwork.Destroy(gameObject);
       }
    }
}

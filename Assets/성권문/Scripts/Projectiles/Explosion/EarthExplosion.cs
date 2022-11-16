using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EarthExplosion : MonoBehaviourPun
{
    [HideInInspector]
    public string enemyTag;

    [HideInInspector]
    public float damage;
    private void OnEnable()
    {
        StartCoroutine(Destruction());
    }

    private IEnumerator Destruction()
    {
        yield return new WaitForSeconds(3f);

        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }

        StopCoroutine(Destruction());
    }
}

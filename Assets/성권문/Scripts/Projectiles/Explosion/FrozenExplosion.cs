using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FrozenExplosion : MonoBehaviourPun
{
    public GameObject shot1;
    public GameObject shot2;
    public GameObject pieceParticle;

    [HideInInspector]
    public string enemyTag;

    [HideInInspector]
    public float damage;

    public int EffectID;
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

    private IEnumerator Start()
    {
        shot1.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        shot1.SetActive(false);
        shot2.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        shot2.SetActive(false);
        pieceParticle.SetActive(true);

        for (int i = 0; i < pieceParticle.transform.childCount; i++)
        {
            PieceExplosion snowflake = pieceParticle.transform.GetChild(i).GetComponent<PieceExplosion>();
            snowflake.damage = 5f;
            snowflake.enemyTag = enemyTag;
            snowflake.EffectID = EffectID;
            pieceParticle.transform.GetChild(i).gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
        }
    }
}

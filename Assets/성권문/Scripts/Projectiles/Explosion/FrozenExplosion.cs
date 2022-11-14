using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenExplosion : MonoBehaviour
{
    public GameObject shot1;
    public GameObject shot2;
    public GameObject pieceParticle;

    public string enemyTag;
    public float damage;

    private void OnEnable()
    {
        Destroy(gameObject, 3f);
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
            pieceParticle.transform.GetChild(i).gameObject.SetActive(true);
            yield return new WaitForSeconds(0.2f);
        }
    }
}

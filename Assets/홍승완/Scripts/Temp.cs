using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp : MonoBehaviour
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public Collider CharactorCol;
    public GameObject ChractorRenderers;

    Animator _ani;

    bool isDead;
    private void Awake()
    {
        _ani = GetComponent<Animator>();
    }

    private void Update()
    {
        float speed = 0f;
        if (Input.GetKeyDown(KeyCode.I))
        {
            isDead = true;
            Debug.Log($"Ä³¸¯ÅÍ Á×À½ : {isDead}");
            if (isDead)
            {
                CharactorCol.enabled = false;
                _ani.SetBool("Die", true);
                StartCoroutine(RemoveRenderer());
            }

        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            speed = 1f;
            _ani.SetFloat("Speed", speed, 0.1f, Time.deltaTime);
        }
    }

    float respawnTime = 5f;
    IEnumerator RemoveRenderer()
    {
        yield return new WaitForSeconds(respawnTime);
        ChractorRenderers.SetActive(false);
    }

}

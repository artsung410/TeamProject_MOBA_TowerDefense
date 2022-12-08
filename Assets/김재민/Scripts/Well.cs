using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System;

public class Well : MonoBehaviourPun
{
    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################

    [SerializeField]
    GameObject healthEffect;

    void Start()
    {
        healthEffect.SetActive(false);
        InvokeRepeating(nameof(RegenerationSwitch), 0, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("�뱺");
    }

    private void RegenerationSwitch()
    {
        Collider[] WeTeam = Physics.OverlapSphere(gameObject.transform.position, 15);

        foreach (Collider col in WeTeam)
        {
            if (col.gameObject.layer == 12 && col.CompareTag(gameObject.tag)) // �ڱ� �ڽ� ����
            {
                continue;
            }

            if (col.CompareTag(gameObject.tag) && col.gameObject.layer == 7 && photonView.IsMine) //�츮���̶� �±� ���� �÷��̾� + �ڱ��ڽſ��Ը� 
            {
                if (col.GetComponent<Health>().health < col.GetComponent<Stats>().maxHealth)
                {
                    photonView.RPC("effectSwich", RpcTarget.All, true);
                    col.GetComponent<Health>().Regenation(250f);

                }
                else
                {
                    photonView.RPC("effectSwich", RpcTarget.All, false);

                }
            }


        }
    }

    [PunRPC]
    private void effectSwich(bool value)
    {
        healthEffect.SetActive(value);
    }
}

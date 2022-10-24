using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class NexusHp : MonoBehaviourPun
{
    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################
   public float CurrentHp;
    public float MaxHp = 1000;
    [SerializeField]
    private Slider _slider;
    private void Awake()
    {
        CurrentHp = MaxHp;
        _slider = GetComponentInChildren<Slider>();
    }
 

    // Update is called once per frame
    void Update()
    {
        _slider.value = CurrentHp / MaxHp;
        if (_slider == null)
        {
            return;
        }

        if (transform == null || transform.gameObject == null || this == null)
        {
            return;
        }

    }

    public void TakeOnDagmage(float Damage)
    {

        photonView.RPC("RPC_TakeDamage", RpcTarget.All, Damage);
    }

    [PunRPC]
    private void RPC_TakeDamage(float Damage)
    {
        CurrentHp -= Damage;
        if (CurrentHp <= 0)
        {
            Destroy(gameObject);
        }

    }

}

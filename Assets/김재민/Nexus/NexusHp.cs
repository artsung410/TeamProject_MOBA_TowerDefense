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
    public float MaxHp;
    public GameObject destroyParticle;
    [SerializeField]
    private Slider _slider;
    private void Awake()
    {
        CurrentHp = MaxHp;
       
    }
 

    // Update is called once per frame
    void Update()
    {
        if(gameObject.activeSelf == false)
        {
            return;
        }
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
            PlayerHUD.Instance.ActivationGameWinUI_Nexus(gameObject.tag);
            PhotonNetwork.Instantiate(destroyParticle.name, new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), transform.rotation);
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            transform.GetChild(0).gameObject.SetActive(false);
            
        }

    }

}

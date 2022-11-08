using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.UI;

public class NexusHp : MonoBehaviourPun
{
    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################
    public static event Action<NexusHp, Sprite> nexusMouseDownEvent = delegate { };

    public float CurrentHp;
    public float MaxHp;
    private bool isDie;
    public GameObject destroyParticle;
    [SerializeField]
    private Slider _slider;
    public Sprite nexusSprite;
  
    GameObject _player;
    [SerializeField]
    GameObject healthEffect;
    WaitForSeconds Dealay100 = new WaitForSeconds(1);
    Outline _outline;
    string myTag;
    private void Awake()
    {
        _outline = GetComponent<Outline>(); 
        CurrentHp = MaxHp;
        _player = null;
        
        _outline.enabled = false;
        _outline.OutlineWidth = 8f;
        
    }


    private void OnEnable()
    {
        
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 1 && photonView.IsMine)
            {
                gameObject.tag = "Blue";

            }
        }
        else
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 2 && photonView.IsMine)
            {
                gameObject.tag = "Red";
            }
        }
        
    }

    private void Start()
    {
        healthEffect.SetActive(false);
        InvokeRepeating("RegenerationSwich", 0, 1f);
    }


    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf == false)
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
        if (isDie == false)
        {

            CurrentHp -= Damage;
            if (CurrentHp <= 0)
            {
                isDie = true;
                PlayerHUD.Instance.ActivationGameWinUI_Nexus(gameObject.tag);
                PhotonNetwork.Instantiate(destroyParticle.name, new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), transform.rotation);
                gameObject.GetComponent<MeshRenderer>().enabled = false;
                transform.GetChild(0).gameObject.SetActive(false);

            }
        }
    }

    private void OnMouseDown()
    {
        nexusMouseDownEvent.Invoke(this, nexusSprite);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, 15f);

    }

    private void RegenerationSwich()
    {
        Collider[] WeTeam = Physics.OverlapSphere(gameObject.transform.position, 15);

        foreach (Collider col in WeTeam)
        {
            if (col.gameObject.layer == 12 && col.CompareTag(gameObject.tag)) // 자기 자신 제외
            {
                continue;
            }

            if (col.CompareTag(gameObject.tag) && col.gameObject.layer == 7 && photonView.IsMine) //우리팀이랑 태그 같고 플레이어 + 자기자신에게만 
            {
                _player = col.gameObject;
                if (_player.GetComponent<Health>().health < _player.GetComponent<Stats>().StartHealth)
                {
                    photonView.RPC("effectSwich", RpcTarget.All, true);
                   
                 
                    _player.GetComponent<Health>().Regenation(25f);
              
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

    private void OnMouseEnter()
    {

        if (photonView.IsMine) // 자기 자신이면 켜주고  색 그린
        {

            _outline.OutlineColor = Color.green;
            _outline.enabled = true; // 켜주고
        }
        else
        {

            _outline.OutlineColor = Color.red;
            _outline.enabled = true;
        }

    }

    private void OnMouseExit()
    {
        _outline.enabled = false;
    }



}

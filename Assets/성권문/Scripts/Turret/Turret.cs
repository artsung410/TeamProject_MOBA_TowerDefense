using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com         
// ###############################################

public class Turret : MonoBehaviourPun
{
    public float currentHealth;
    public float maxHealth;
    public Image healthbarImage;
    public GameObject ui;
    public GameObject destroyParticle;
    public float destorySpeed;

    [Header("타겟 TAG")]
    public string enemyTag;
    private GameObject newDestroyParticle;

    protected void Awake()
    {
        PlayerHUD.onGameEnd += Destroy_gameEnd;
    }

    protected void OnEnable()
    {
        currentHealth = maxHealth;
        GameManager.Instance.CurrentTurrets.Add(gameObject);

        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 1 && photonView.IsMine)
            {
                gameObject.tag = "Blue";
                enemyTag = "Red";
            }

            else
            {
                gameObject.tag = "Red";
                enemyTag = "Blue";
            }
        }

        else
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 2 && photonView.IsMine)
            {
                gameObject.tag = "Red";
                enemyTag = "Blue";
            }

            else
            {
                gameObject.tag = "Blue";
                enemyTag = "Red";
            }
        }
    }

    public void Damage(float damage)
    {
        //Debug.Log("Damage 적용");

        // 게임 끝나면 정지
        if (GameManager.Instance.isGameEnd == true)
        {
            return;
        }

        photonView.RPC("TakeDamage", RpcTarget.All, damage);
    }

    [PunRPC]
    public void TakeDamage(float damage)
    {
        //Debug.Log("Damage RPC적용");

        currentHealth -= damage;
        healthbarImage.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            photonView.RPC("Destroy", RpcTarget.All);
            return;
        }
    }

    public void Destroy_gameEnd()
    {
        if (this == null)
        {
            return;
        }

        photonView.RPC("Destroy", RpcTarget.All);
    }

    [PunRPC]
    public void Destroy()
    {
        StartCoroutine(Destructing());
        newDestroyParticle = PhotonNetwork.Instantiate(destroyParticle.name, new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), transform.rotation);
        StartCoroutine(Destruction(newDestroyParticle));
    }

    IEnumerator Destructing()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.01f);
            transform.Translate(Vector3.down * Time.deltaTime * destorySpeed);

            if (transform.position.y < -10)
            {
                StopCoroutine(Destructing());

                if (newDestroyParticle != null)
                {
                    StopCoroutine(Destructing());
                    StopCoroutine(Destruction(newDestroyParticle));
                }
            }
        }
    }

    IEnumerator Destruction(GameObject particle)
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(particle);
        gameObject.SetActive(false);
    }
}

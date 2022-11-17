using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerRespawn : MonoBehaviourPun
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public GameObject Renderer;

    public Health health;
    public Slider hpBar;
    [SerializeField] Transform[] respawnPosition;

    [SerializeField] float respawnTime;
    Vector3 playerRespawnPosition;


    private void Awake()
    {
        //health = transform.GetChild(2).GetComponent<Health>();
        //hpBar = transform.GetChild(0).GetChild(0).GetComponent<Slider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        respawnTime = 5f;

        StartCoroutine(RespawnPositionInit());
    }
    IEnumerator RespawnPositionInit()
    {
        yield return new WaitForSeconds(1f);

        if (health.gameObject.CompareTag("Blue"))
        {
    // TODO : 블루진영캐릭터 리스폰시 땅에 파묻힘 -> 리스폰장소 y축으로 2 올린상태 정확한원인은 잘 모르겠음
            playerRespawnPosition = new Vector3(GameManager.Instance.spawnPositions[0].position.x, 2f, GameManager.Instance.spawnPositions[0].position.z);

        }
        else if (health.gameObject.CompareTag("Red"))
        {
            respawnPosition[1] = GameManager.Instance.spawnPositions[1];
            playerRespawnPosition = respawnPosition[1].position;
        }
    }


    void Update()
    {
        
            Respawn();
        

    }

    float elapsedTime;
    public void Respawn()
    {
        if (health.isDeath)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= respawnTime)
            {
                // TODO : MissingReferenceException: The object of type 'Health' has been destroyed but you are still trying to access it.
                // 리스폰위치 할당
                health.gameObject.transform.position = playerRespawnPosition;
                elapsedTime = 0f;
                health.gameObject.SetActive(true);
                hpBar.gameObject.SetActive(true);
                Renderer.SetActive(true);
                health.isDeath = false;
            }
        }
    }



}

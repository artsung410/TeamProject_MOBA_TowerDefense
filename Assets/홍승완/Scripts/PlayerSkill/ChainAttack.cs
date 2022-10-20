using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChainAttack : MonoBehaviourPun, IDamageable
{

    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public float HoldingTime;

    public ParticleSystem effect;

    Stats playerStats;

    Quaternion quaternion;
    float elapsedTime;


    private void Awake()
    {
        playerStats = GameObject.FindObjectOfType<Stats>();
        effect = gameObject.GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        elapsedTime = 0f;
        quaternion = playerStats.gameObject.transform.localRotation;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 스킬위치는 플레이어를 따라온다
        transform.position = playerStats.gameObject.transform.position;
        transform.rotation = quaternion;
        SkillHoldingTime(HoldingTime);
    }

    public void TakeDamage(float damage)
    {

    }

    private void OnTriggerEnter(Collider other)
    { 
      
    }

    //float debuffSpeed = 3f;
    public void SkillHoldingTime(float time)
    {
        elapsedTime += Time.deltaTime;
    
        // 지속시간동안 플레이어가 느려진다
        playerStats.MoveSpeed = 3f;

        // 지속시간동안 플레이어는 스킬방향만 바라본다
        playerStats.gameObject.transform.rotation = quaternion;

        // 지속시간이 끝나면 사라진다
        if (elapsedTime >= time)
        {
            playerStats.MoveSpeed = 20f;
            PhotonNetwork.Destroy(gameObject);
        }
    }

    public void SkillCoolTime(float time)
    {

    }

    
}

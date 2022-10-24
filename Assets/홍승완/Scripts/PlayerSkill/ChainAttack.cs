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

        // 내가 쓴 스킬이라는걸 어떻게 알지

        // 현재 문제점
        // 로컬플레이어가 스킬사용시 리모트 플레이어에게서 스킬이 나감
        // 조종중인 플레이어가 스킬을 쓰면 내 화면에선 상대편위치에서 스킬나감
        // 상대방 화면에선 내 위치에서 스킬이 나감
        // 왜지?
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
            playerStats.MoveSpeed = 10f;

            // Failed to 'network-remove' GameObject. Client is neither owner nor masterClient taking over for owner who left 오류발생
            // 해결법 : photonView.IsMine 조건 추가
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    public void SkillCoolTime(float time)
    {

    }


}

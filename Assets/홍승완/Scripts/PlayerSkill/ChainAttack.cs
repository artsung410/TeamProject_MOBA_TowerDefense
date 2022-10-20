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
        // ��ų��ġ�� �÷��̾ ����´�
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
    
        // ���ӽð����� �÷��̾ ��������
        playerStats.MoveSpeed = 3f;

        // ���ӽð����� �÷��̾�� ��ų���⸸ �ٶ󺻴�
        playerStats.gameObject.transform.rotation = quaternion;

        // ���ӽð��� ������ �������
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

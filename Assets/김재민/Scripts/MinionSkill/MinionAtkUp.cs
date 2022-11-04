using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class MinionAtkUp : SkillHandler
{
    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################
    private string myTag;
    private float elapsedTime;
    private float holdlingTime;
    private float prevDamage;
    private float atkBuff = 20f;


    // �������ӽð�
    public override void SkillHoldingTime(float time)
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= time)
        {
                minionBuffReset();
                PhotonNetwork.Destroy(gameObject);
           
        }
    }

    public override void SkillUpdatePosition()
    {

    }


    void Start()
    {
        if (_ability == null)
        {
            return;
        }
        // ���� �±� ã��
        Debug.Log($"{getMytag(_ability)}");
        minionBuff(atkBuff); // �̴Ͼ� ���� ����

    }

    private void Update()
    {
        if (_ability == null)
        {
            return;
        }
        //�÷��̾� ������ ����
        if (_ability.gameObject.GetComponent<Health>().isDeath == true)
        {
                minionBuffReset();
                PhotonNetwork.Destroy(gameObject);   
        }
        SkillHoldingTime(20f);
    }
    // �±� ã���� 
    

    private void minionBuff(float attackUp)
    {
        photonView.RPC("RPC_MinionBuff", RpcTarget.All, attackUp);
    }

    private void minionBuffReset()
    {
        photonView.RPC("RPC_MinionDamageReset", RpcTarget.All);
    }



    // �츮�̴Ͼ� ���ݷ� ���� 
    [PunRPC]
    private void RPC_MinionBuff(float attackUp)
    {
        GameObject[] Minions = GameObject.FindGameObjectsWithTag(getMytag(_ability));
        foreach (GameObject minion in Minions)
        {
            if (minion.layer == 8)
            {
                prevDamage = minion.GetComponent<Enemybase>().Damage;
                minion.GetComponent<Enemybase>().Damage += attackUp;
            }
        }
    }
    // �츮�̴Ͼ� ���ݷ� ����
    [PunRPC]
    private void RPC_MinionDamageReset()
    {
        GameObject[] Minions = GameObject.FindGameObjectsWithTag(getMytag(_ability));
        foreach (GameObject minion in Minions)
        {
            if (minion.layer == 8)
            {
                minion.GetComponent<Enemybase>().Damage = prevDamage;
            }
        }
    }



}

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
    private float prevAtkSpeed;
    private float atkBuff;
    private float atkSpeedBuff;
    public ScriptableObject buff;

    // �������ӽð�
    public override void SkillHoldingTime(float time)
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= Data.LockTime)
        {
            _ability.OnLock(true);
        }

        if (elapsedTime >= time)
        {
            minionBuffReset();
            _ability.OnLock(false);
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
        if (photonView.IsMine)
        {
        atkBuff = Data.Value_1;
        atkSpeedBuff = Data.Value_2;
        minionBuff(atkBuff, atkSpeedBuff); // �̴Ͼ� ���� ����

        }
        //BuffManager.Instance.AddBuff((BuffData)buff);
        BuffManager.Instance.AssemblyBuff();
        _ability.OnLock(false);
        
    }

    private void Update()
    {
        if (_ability == null)
        {
            return;
        }
        //�÷��̾� ������ ����
        if(photonView.IsMine)
        {
        SkillHoldingTime(Data.HoldingTime);
            Debug.Log($"{atkSpeedBuff}, {atkBuff} "); 
            
        }
    }
    // �±� ã���� 


    private void minionBuff(float attackUp, float atkSpeedBuff)
    {
        photonView.RPC("RPC_MinionBuff", RpcTarget.All, attackUp, atkSpeedBuff);
    }

    private void minionBuffReset()
    {
        photonView.RPC("RPC_MinionDamageReset", RpcTarget.All);
    }



    // �츮�̴Ͼ� ���ݷ� ���� 
    [PunRPC]
    private void RPC_MinionBuff(float attackUp,float atkSpeedBuff)
    {
        GameObject[] Minions = GameObject.FindGameObjectsWithTag(GetMytag(_ability));
        foreach (GameObject minion in Minions)
        {
            if (minion.layer == 8)
            {
                prevAtkSpeed = minion.GetComponent<Enemybase>().AttackSpeed;
                prevDamage = minion.GetComponent<Enemybase>().Damage;
                minion.GetComponent<Enemybase>().Damage += attackUp;
                minion.GetComponent<Enemybase>().AtkSpeedUp(atkSpeedBuff);

            }
        }
    }

    // �츮�̴Ͼ� ���ݷ� ����
    [PunRPC]
    private void RPC_MinionDamageReset()
    {
        GameObject[] Minions = GameObject.FindGameObjectsWithTag(GetMytag(_ability));
        foreach (GameObject minion in Minions)
        {
            if (minion.layer == 8)
            {
                minion.GetComponent<Enemybase>().Damage = prevDamage;
                minion.GetComponent<Enemybase>().atkSpeedReset(prevAtkSpeed);
            }
        }
    }


}

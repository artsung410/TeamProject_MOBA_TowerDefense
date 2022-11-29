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

    // 버프지속시간
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


        // 나의 태그 찾음
        if (photonView.IsMine)
        {
        atkBuff = Data.Value_1;
        atkSpeedBuff = Data.Value_2;
        minionBuff(atkBuff, atkSpeedBuff); // 미니언 버프 적용

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
        //플레이어 죽으면 삭제
        if(photonView.IsMine)
        {
        SkillHoldingTime(Data.HoldingTime);
            Debug.Log($"{atkSpeedBuff}, {atkBuff} "); 
            
        }
    }
    // 태그 찾아줌 


    private void minionBuff(float attackUp, float atkSpeedBuff)
    {
        photonView.RPC("RPC_MinionBuff", RpcTarget.All, attackUp, atkSpeedBuff);
    }

    private void minionBuffReset()
    {
        photonView.RPC("RPC_MinionDamageReset", RpcTarget.All);
    }



    // 우리미니언 공격력 증가 
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

    // 우리미니언 공격력 리셋
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

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
        minionBuff(atkBuff); // 미니언 버프 적용
        //BuffManager.Instance.AddBuff((BuffData)buff);
        BuffManager.Instance.AssemblyBuff();
        _ability.OnLock(true);

    }

    private void Update()
    {
        if (_ability == null)
        {
            return;
        }
        //플레이어 죽으면 삭제
        SkillHoldingTime(Data.HoldingTime);
    }
    // 태그 찾아줌 


    private void minionBuff(float attackUp)
    {
        photonView.RPC("RPC_MinionBuff", RpcTarget.All, attackUp);
    }

    private void minionBuffReset()
    {
        photonView.RPC("RPC_MinionDamageReset", RpcTarget.All);
    }



    // 우리미니언 공격력 증가 
    [PunRPC]
    private void RPC_MinionBuff(float attackUp)
    {
        GameObject[] Minions = GameObject.FindGameObjectsWithTag(GetMytag(_ability));
        foreach (GameObject minion in Minions)
        {
            if (minion.layer == 8)
            {
                prevDamage = minion.GetComponent<Enemybase>().Damage;
                minion.GetComponent<Enemybase>().Damage += attackUp;
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
            }
        }
    }



}

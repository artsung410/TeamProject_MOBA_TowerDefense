using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System;

public class HeroAbility : MonoBehaviourPun
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public GameObject[] AbilityPrefabs;
    public Transform skillSpawn;
    public SkillCoolTimeManager coolTimeManager;

    GameObject go;

    #region private ������


    #endregion

    void Awake()
    {
        int count = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>().skillItems.Count;

        if (count == 0)
        {
            return;
        }

        AbilityPrefabs = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            // Ʈ���̸� �ȿ� �ִ� ������ ������ ���ӻ󿡼� ��������
            AbilityPrefabs[i] = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>().skillItems[i].itemModel;
        }
    }


    void Update()
    {
        // ���� ������ ����
        if (GameManager.Instance.isGameEnd == true)
        {
            return;
        }

        if (photonView.IsMine)
        {
            AbilityQ();
            AbilityW();
            AbilityE();
            AbilityR();

            if (go != null)
            {
                go.GetComponent<SkillHandler>().GetPlayerPos(this);
                go.GetComponent<SkillHandler>().GetPlayerStatus(this.GetComponent<Stats>());
                go.GetComponent<SkillHandler>().GetMousePos(this.GetComponent<PlayerBehaviour>());
            }
        }
    }

    private void AbilityQ()
    {
        if (AbilityPrefabs[0] == null || AbilityPrefabs[0].GetComponent<SkillHandler>() == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Q) && coolTimeManager.isCoolDown[0] == false)
        {
            coolTimeManager.isCoolDown[0] = true;
            coolTimeManager.coolTimeImgs[0].fillAmount = 1;

            AbilityPrefabs[0].GetComponent<SkillHandler>().SetDamage = SkillManager.Instance.Atk[0];
            AbilityPrefabs[0].GetComponent<SkillHandler>().SetHodingTime = SkillManager.Instance.HoldingTime[0];
            AbilityPrefabs[0].GetComponent<SkillHandler>().SetRange = SkillManager.Instance.Range[0];

            go = PhotonNetwork.Instantiate(AbilityPrefabs[0].name, skillSpawn.position, Quaternion.identity);

        }


    }


    private void AbilityW()
    {
        if (AbilityPrefabs[1] == null || AbilityPrefabs[1].GetComponent<SkillHandler>() == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.W) && coolTimeManager.isCoolDown[1] == false)
        {
            coolTimeManager.isCoolDown[1] = true;
            coolTimeManager.coolTimeImgs[1].fillAmount = 1;


            AbilityPrefabs[1].GetComponent<SkillHandler>().SetDamage = SkillManager.Instance.Atk[1];
            AbilityPrefabs[1].GetComponent<SkillHandler>().SetHodingTime = SkillManager.Instance.HoldingTime[1];
            AbilityPrefabs[1].GetComponent<SkillHandler>().SetRange = SkillManager.Instance.Range[1];


            go = PhotonNetwork.Instantiate(AbilityPrefabs[1].name, skillSpawn.position, Quaternion.identity);
        }

    }

    private void AbilityE()
    {
        if (AbilityPrefabs[2] == null || AbilityPrefabs[2].GetComponent<SkillHandler>() == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.E) && coolTimeManager.isCoolDown[2] == false)
        {
            coolTimeManager.isCoolDown[2] = true;
            coolTimeManager.coolTimeImgs[2].fillAmount = 1;


            AbilityPrefabs[2].GetComponent<SkillHandler>().SetDamage = SkillManager.Instance.Atk[2];
            AbilityPrefabs[2].GetComponent<SkillHandler>().SetHodingTime = SkillManager.Instance.HoldingTime[2];
            AbilityPrefabs[2].GetComponent<SkillHandler>().SetRange = SkillManager.Instance.Range[2];

            go = PhotonNetwork.Instantiate(AbilityPrefabs[2].name, skillSpawn.position, Quaternion.identity);
        }

    }


    private void AbilityR()
    {
        if (AbilityPrefabs[3] == null || AbilityPrefabs[3].GetComponent<SkillHandler>() == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.R) && coolTimeManager.isCoolDown[3] == false)
        {
            coolTimeManager.isCoolDown[3] = true;
            coolTimeManager.coolTimeImgs[3].fillAmount = 1;
            coolTimeManager.CoolTimeCheckR();


            AbilityPrefabs[3].GetComponent<SkillHandler>().SetDamage = SkillManager.Instance.Atk[3];
            AbilityPrefabs[3].GetComponent<SkillHandler>().SetHodingTime = SkillManager.Instance.HoldingTime[3];
            AbilityPrefabs[3].GetComponent<SkillHandler>().SetRange = SkillManager.Instance.Range[3];

            go = PhotonNetwork.Instantiate(AbilityPrefabs[3].name, skillSpawn.position, Quaternion.identity);
        }

    }



}

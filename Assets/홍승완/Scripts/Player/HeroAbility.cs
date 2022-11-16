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
    public PlayerAnimation skillMotion;

    [Header("캐릭터 모델링")]
    public GameObject ChractorRenderer;
    public GameObject ChractorHpBar;
    public Collider ChractorCol;


    SkillManager _skillManager;
    GameObject go;

    //TrojanHorse _trojanHorse;

    #region private 변수들


    #endregion

    void Awake()
    {
        AbilityPrefabs = new GameObject[4];
        _skillManager = SkillManager.Instance;
    }


    private IEnumerator Start()
    {
        while (true)
        {
            try
            {
                if (_skillManager.SkillData.skillIndex.Count == 0)
                {
                    yield break;
                }
                for (int i = 0; i < _skillManager.SkillData.skillIndex.Count; i++)
                {
                    int idx = _skillManager.SkillData.skillIndex[i];
                    AbilityPrefabs[idx] = _skillManager.SkillPF[idx];
                }
                yield break;
            }
            catch (Exception ie)
            {
                print(ie);
                _skillManager = SkillManager.Instance;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    // TODO : 스킬발동중 다른 스킬을 쓰면 쓰던스킬이 취소되게한다(날리는 스킬 제외) or 다른 스킬을 못쓰게한다

    void Update()
    {
        // 게임 끝나면 정지
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
                //go.GetComponent<SkillHandler>().GetAnimation(this.GetComponent<PlayerAnimation>());
                go.GetComponent<SkillHandler>().GetAnimation(skillMotion);
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

    public void CharactorRenderEvent(bool check)
    {
        photonView.RPC(nameof(RendererActivate), RpcTarget.All, check);
    }

    [PunRPC]
    public void RendererActivate(bool bb)
    {
        ChractorRenderer.SetActive(bb);
        ChractorHpBar.SetActive(bb);
        ChractorCol.enabled = bb;
    }
}

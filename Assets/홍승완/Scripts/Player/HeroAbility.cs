//#define SKILL_TEST
//#define OLD_VER
#define PARSE_VER

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

    [Header("스킬PF")]
    public GameObject[] AbilityPrefabs;
    [Header("스킬 테스트용")]
    public GameObject TestAbilityPF;
    public float TestDamage;
    public float TestHoldingTime;
    public float TestRange;
    [Space]
    public Transform skillSpawn; // 스킬나가는 위치
    public SkillCoolTimeManager coolTimeManager;
    public PlayerAnimation skillMotion;

    [Header("캐릭터 모델링")]
    public GameObject ChractorRenderer;
    public GameObject ChractorHpBar;
    public Collider ChractorCol;

    GameObject go;
    PlayerBehaviour _behaviour;

    void Awake()
    {
        AbilityPrefabs = new GameObject[4];
        _behaviour = GetComponent<PlayerBehaviour>();
    }

    private void OnEnable()
    {
        OnLock(false);
    }

    private IEnumerator Start()
    {
        while (true)
        {
#if OLD_VER
            yield return new WaitForSeconds(0.5f);
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

#endif

#if PARSE_VER
            yield return new WaitForSeconds(0.5f);
            try
            {
                Debug.Log("try");
                // TODO : Q스킬 현재 제외(i = 1인 이유)

                for (int i = 0; i < 4; i++)
                {
                    AbilityPrefabs[i] = SkillManager.Instance.Datas[i].Name;
                }
                yield break;
            }
            catch (Exception ie)
            {
                Debug.Log($"catch {ie}");
            }
#endif
        }
    }

    public bool isActive; // 스킬사용중 -> true

    void Update()
    {
        // 게임 끝나면 정지
        if (GameManager.Instance.isGameEnd == true)
        {
            return;
        }

        if (photonView.IsMine)
        {
            if (isActive == true || _behaviour.isStun == true)
            {
                return;
            }

            AbilityQ();
            AbilityW();
            AbilityE();
            AbilityR();

#if SKILL_TEST

            AbilityTest();
#endif

            if (go != null)
            {
                go.GetComponent<SkillHandler>().GetPlayerPos(this);
                go.GetComponent<SkillHandler>().GetPlayerStatus(this.GetComponent<Stats>());
                go.GetComponent<SkillHandler>().GetMousePos(this.GetComponent<PlayerBehaviour>());
                go.GetComponent<SkillHandler>().GetAnimation(skillMotion);
            }
        }
    }

#if SKILL_TEST

    private void AbilityTest()
    {
        if (TestAbilityPF == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            TestAbilityPF.GetComponent<SkillHandler>().SetDamage = TestDamage;
            TestAbilityPF.GetComponent<SkillHandler>().SetHodingTime = TestHoldingTime;
            TestAbilityPF.GetComponent<SkillHandler>().SetRange = TestRange;

            go = Instantiate(TestAbilityPF, skillSpawn);
        }

    }
#endif

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

#if OLD_VER
            AbilityPrefabs[0].GetComponent<SkillHandler>().SetDamage = SkillManager.Instance.Atk[0];
            AbilityPrefabs[0].GetComponent<SkillHandler>().SetHodingTime = SkillManager.Instance.HoldingTime[0];
            AbilityPrefabs[0].GetComponent<SkillHandler>().SetRange = SkillManager.Instance.Range[0];
            AbilityPrefabs[0].GetComponent<SkillHandler>().SetLockTime = SkillManager.Instance.LockTime[0];

#endif

#if PARSE_VER
            AbilityPrefabs[0].GetComponent<SkillHandler>().Data = SkillManager.Instance.Datas[0];
#endif

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

#if OLD_VER
            AbilityPrefabs[1].GetComponent<SkillHandler>().SetDamage = SkillManager.Instance.Atk[1];
            AbilityPrefabs[1].GetComponent<SkillHandler>().SetHodingTime = SkillManager.Instance.HoldingTime[1];
            AbilityPrefabs[1].GetComponent<SkillHandler>().SetRange = SkillManager.Instance.Range[1];
            AbilityPrefabs[1].GetComponent<SkillHandler>().SetLockTime = SkillManager.Instance.LockTime[1];

#endif

#if PARSE_VER
            AbilityPrefabs[1].GetComponent<SkillHandler>().Data = SkillManager.Instance.Datas[1];
#endif

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

#if OLD_VER
            AbilityPrefabs[2].GetComponent<SkillHandler>().SetDamage = SkillManager.Instance.Atk[2];
            AbilityPrefabs[2].GetComponent<SkillHandler>().SetHodingTime = SkillManager.Instance.HoldingTime[2];
            AbilityPrefabs[2].GetComponent<SkillHandler>().SetRange = SkillManager.Instance.Range[2];
            AbilityPrefabs[2].GetComponent<SkillHandler>().SetLockTime = SkillManager.Instance.LockTime[2];

#endif

#if PARSE_VER
            AbilityPrefabs[2].GetComponent<SkillHandler>().Data = SkillManager.Instance.Datas[2];
#endif

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

#if OLD_VER
            AbilityPrefabs[3].GetComponent<SkillHandler>().SetDamage = SkillManager.Instance.Atk[3];
            AbilityPrefabs[3].GetComponent<SkillHandler>().SetHodingTime = SkillManager.Instance.HoldingTime[3];
            AbilityPrefabs[3].GetComponent<SkillHandler>().SetRange = SkillManager.Instance.Range[3];
            AbilityPrefabs[3].GetComponent<SkillHandler>().SetLockTime = SkillManager.Instance.LockTime[3];

#endif

#if PARSE_VER
            AbilityPrefabs[3].GetComponent<SkillHandler>().Data = SkillManager.Instance.Datas[3];
#endif

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

    public void OnLock(bool isLock)
    {
        //Debug.Log("onlock 실행");
        photonView.RPC(nameof(RPC_Lock), RpcTarget.All, isLock);
    }

    [PunRPC]
    public void RPC_Lock(bool isLock)
    {
        isActive = isLock;
    }
}

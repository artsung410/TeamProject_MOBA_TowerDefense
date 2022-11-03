using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Leap : SkillHandler
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    // TODO : 착지한 지점에서 이펙트 동기화 문제(리모트캐릭터에선 이펙트 활성화 안되는중)

    public GameObject effect;

    #region Private 변수들

    Quaternion quaternion;
    float elapsedTime;
    string enemyTag;
    Vector3 mouseDir;

    private float HoldingTime;
    private float Damage;
    private float Range;
    private float Speed;
    #endregion

    private void Awake()
    {
    }

    private void OnEnable()
    {
        elapsedTime = 0f;
        Damage = SetDamage;
        HoldingTime = SetHodingTime;
        Range = SetRange;

        isArive = false;
        isAttack = false;

        effect.SetActive(isArive);
        
    }

    private void Start()
    {
        if (_ability == null)
        {
            return;
        }

        TagProcessing(_ability);
        LookMouseCursor();

        CheckDist();
    }

    private void CheckDist()
    {
        mousePos = new Vector3(hit.point.x, _ability.transform.position.y, hit.point.z);

        if (Vector3.Distance(_behaviour.transform.position, mousePos) >= Range)
        {
            start = _behaviour.transform.position;
            end = _behaviour.transform.forward;
            leapPos = (start + end.normalized * Range);
        }
        else
        {
            leapPos = mousePos;
        }
    }

    RaycastHit hit;
    public void LookMouseCursor()
    {
        // 마우스 방향에서 사용
        if (Physics.Raycast(_behaviour.ray, out hit))
        {
            mouseDir = new Vector3(hit.point.x, _ability.transform.position.y, hit.point.z) - _ability.transform.position;

            _ability.transform.forward = mouseDir;
            quaternion = _ability.transform.localRotation;
        }
    }
    private void TagProcessing(HeroAbility ability)
    {

        if (ability.CompareTag("Blue"))
        {
            enemyTag = "Red";
        }
        else if (ability.CompareTag("Red"))
        {
            enemyTag = "Blue";
        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            SkillUpdatePosition();
            SkillHoldingTime(HoldingTime);
            effect.SetActive(isArive);
        }
    }

    Vector3 mousePos;
    Vector3 leapPos;

    Vector3 start;
    Vector3 end;

    // SMS -----------------------------------------------------------------
    public Animator animator;
    Vector3 velo = Vector3.forward; // 임시

    private IEnumerator LeapAttackAnimationStart()
    {
        animator.SetBool("JumpAttack", true);
        yield return new WaitForSeconds(0.5f);
    }
    private IEnumerator LeapAttackAnimationEnd()
    {
        yield return new WaitForSeconds(3f);
        animator.SetBool("JumpAttack", false);
    }


    // ---------------------------------------------------------------------
    public override void SkillUpdatePosition()
    {
        transform.position = leapPos;
    }

    public override void SkillHoldingTime(float time)
    {
        elapsedTime += Time.deltaTime;

        StartCoroutine(LeapAttackAnimationStart());

        // 지속시간동안 플레이어가 지정한 장소로 도약한다
        _behaviour.transform.position = Vector3.Lerp(_behaviour.transform.position, leapPos, time);

        StartCoroutine(LeapAttackAnimationEnd());

        // 원래 위치로 돌아가지 않도록 도착지를 최종목적지로 설정한다
        _behaviour.ForSkillAgent(leapPos);

        // 착지시 주변에 데미지를 준다(한번만 호출)
        if (Vector3.Distance(_behaviour.transform.position, leapPos) <= 0.1f)
        {
            isArive = true;
            StompDamage();
        }

        if (elapsedTime >= time)
        {
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    bool isArive;
    bool isAttack;
    Collider[] enemies;
    public void StompDamage()
    {
        if (isArive == true && isAttack == false)
        {
            // 이 경우 데미지 처리
            if (photonView.IsMine)
            {
                enemies = Physics.OverlapSphere(this.transform.position, 3f);
                //Debug.Log($"배열의 길이 : {enemies.Length}");
                if (enemies.Length > 0)
                {
                    foreach (Collider target in enemies)
                    {
                        if (target.CompareTag(enemyTag))
                        {
                            isAttack = true;
                            SkillDamage(Damage, target.gameObject);
                        }
                    }
                }

            }
        }
        return;
    }

}

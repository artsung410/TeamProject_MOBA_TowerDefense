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

        effect.SetActive(false);

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

        _ani.animator.SetBool("JumpAttack", true);

    }

    Vector3 startPos;
    Vector3 endPos;
    Vector3 mousePos;
    Vector3 leapPos;
    private void CheckDist()
    {
        mousePos = new Vector3(hit.point.x, _ability.transform.position.y, hit.point.z);

        if (Vector3.Distance(_behaviour.transform.position, mousePos) >= Range)
        {
            startPos = _behaviour.transform.position;
            endPos = _behaviour.transform.forward;
            leapPos = (startPos + endPos * Range);
        }
        else
        {
            leapPos = mousePos;
        }
        Debug.Log($"startPos : {startPos}\n" +
            $"startPos.normalized : {startPos.normalized} \n" +
            $"startPos.normalized.magnitude : {startPos.normalized.magnitude}\n" +
            $"startPos.normalized * Range : {startPos.normalized * Range}\n" +
            $"endPos : {endPos}\n" +
            $"endPos.magnitude : {endPos.magnitude}\n" +
            $"leapPos : {leapPos}\n" +
            $"(start - leap)크기 : {(startPos - leapPos).magnitude}\n" +
            $"start와leap사이 거리 : {Vector3.Distance(startPos, leapPos)} \n");
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

            // 지속시간동안 플레이어가 지정한 장소로 도약한다
            _behaviour.transform.position = Vector3.Slerp(_behaviour.transform.position, leapPos, Time.deltaTime * 2f);

            // 원래 위치로 돌아가지 않도록 도착지를 최종목적지로 설정한다
            _behaviour.ForSkillAgent(leapPos);

            // 착지시 주변에 데미지를 준다(한번만 호출)
            if (Vector3.Distance(_behaviour.transform.position, leapPos) <= 0.1f)
            {
                Debug.Log($"거리 : {Vector3.Distance(_behaviour.transform.position, leapPos)}");
                isArive = true;
                effect.SetActive(true);
                _ani.animator.SetBool("JumpAttack", false);

                // TODO : 공중에서 안내려오는 버그 해결할것
                Debug.Log($"_behaviour.transform.position : {_behaviour.transform.position}\n" +
                    $"leapPos : {leapPos}\n" +
                    $"Distance : {Vector3.Distance(_behaviour.transform.position, leapPos)}");
                StompDamage();
                SkillHoldingTime(HoldingTime);
            }

        }

    }






    public override void SkillUpdatePosition()
    {
        this.transform.position = leapPos;
    }

    public override void SkillHoldingTime(float time)
    {
        elapsedTime += Time.deltaTime;

        //StartCoroutine(LeapAttackAnimationStart());



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
            Debug.Log("check");
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

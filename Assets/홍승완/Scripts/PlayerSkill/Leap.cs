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

    // TODO : ������ �������� ����Ʈ ����ȭ ����(����Ʈĳ���Ϳ��� ����Ʈ Ȱ��ȭ �ȵǴ���)

    public GameObject effect;

    #region Private ������

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
            $"(start - leap)ũ�� : {(startPos - leapPos).magnitude}\n" +
            $"start��leap���� �Ÿ� : {Vector3.Distance(startPos, leapPos)} \n");
    }

    RaycastHit hit;
    public void LookMouseCursor()
    {
        // ���콺 ���⿡�� ���
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

            // ���ӽð����� �÷��̾ ������ ��ҷ� �����Ѵ�
            _behaviour.transform.position = Vector3.Slerp(_behaviour.transform.position, leapPos, Time.deltaTime * 2f);

            // ���� ��ġ�� ���ư��� �ʵ��� �������� ������������ �����Ѵ�
            _behaviour.ForSkillAgent(leapPos);

            // ������ �ֺ��� �������� �ش�(�ѹ��� ȣ��)
            if (Vector3.Distance(_behaviour.transform.position, leapPos) <= 0.1f)
            {
                Debug.Log($"�Ÿ� : {Vector3.Distance(_behaviour.transform.position, leapPos)}");
                isArive = true;
                effect.SetActive(true);
                _ani.animator.SetBool("JumpAttack", false);

                // TODO : ���߿��� �ȳ������� ���� �ذ��Ұ�
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
            // �� ��� ������ ó��
            if (photonView.IsMine)
            {
                enemies = Physics.OverlapSphere(this.transform.position, 3f);
                //Debug.Log($"�迭�� ���� : {enemies.Length}");
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

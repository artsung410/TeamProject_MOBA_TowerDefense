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
    Vector3 velo = Vector3.forward; // �ӽ�

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

        // ���ӽð����� �÷��̾ ������ ��ҷ� �����Ѵ�
        _behaviour.transform.position = Vector3.Lerp(_behaviour.transform.position, leapPos, time);

        StartCoroutine(LeapAttackAnimationEnd());

        // ���� ��ġ�� ���ư��� �ʵ��� �������� ������������ �����Ѵ�
        _behaviour.ForSkillAgent(leapPos);

        // ������ �ֺ��� �������� �ش�(�ѹ��� ȣ��)
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

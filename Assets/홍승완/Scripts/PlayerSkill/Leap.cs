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


    GameObject _damageZone;
    GameObject _obstacleDetector;

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
        _damageZone = GetComponentInChildren<SphereCollider>().gameObject;
        _obstacleDetector = transform.GetChild(1).gameObject;
    }

    private void OnEnable()
    {
        elapsedTime = 0f;
        Damage = SetDamage;
        HoldingTime = SetHodingTime;
        Range = SetRange;

        //isArive = false;
        //isAttack = false;

        _damageZone.SetActive(false);
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

    bool isArive;
    private void Update()
    {
        if (photonView.IsMine)
        {
            SkillUpdatePosition();

            // ���ӽð����� �÷��̾ ������ ��ҷ� �̵��Ѵ� => ������ �ִϸ��̼� ó��
            _behaviour.transform.position = Vector3.Slerp(_behaviour.transform.position, leapPos, Time.deltaTime * 2.5f);

            // ���� ��ġ�� ���ư��� �ʵ��� �������� ������������ �����Ѵ�
            _behaviour.ForSkillAgent(leapPos);

            // ������ �ֺ��� �������� �ش�(�ѹ��� ȣ��)
            if (Vector3.Distance(_behaviour.transform.position, leapPos) <= 0.1f)
            {
                //_damageZone.SetActive(true);
                photonView.RPC(nameof(Activate), RpcTarget.All);
                _ani.animator.SetBool("JumpAttack", false);
                isArive = true;
            }

            if (isArive)
            {
                SkillHoldingTime(HoldingTime);
            }
        }

    }


    public override void SkillUpdatePosition()
    {
        this.transform.position = _behaviour.transform.position;
        transform.forward = _behaviour.transform.forward;
    }

    public override void SkillHoldingTime(float time)
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= time)
        {
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine)
        {
            if (other.CompareTag(enemyTag))
            {
                SkillDamage(Damage, other.gameObject);
            }
        }
    }

    // TODO : �浹�� ���ڸ����� �������� ó���Ͽ����� ���� ���� ����� �ִ��� �����غ���?
    private void OnCollisionEnter(Collision collision)
    {
        if (photonView.IsMine)
        {
            // �浹�ϸ� 
            // �ٴڿ� ������(������ġ)

            // �÷��̾� �ڽ��� �����ǰ��־ ����ó������ ��
            if (collision.gameObject.tag == _ability.tag && collision.gameObject.layer == 7)
            {
                return;
            }

            isArive = true;
            _behaviour.transform.position = transform.position;
            photonView.RPC(nameof(Activate), RpcTarget.All);
            _ani.animator.SetBool("JumpAttack", false);
            _behaviour.ForSkillAgent(transform.position);
        }
    }

    // ������ �������� ����Ʈ ����ȭ ����(����Ʈĳ���Ϳ��� ����Ʈ Ȱ��ȭ �ȵǴ���) => RPC�� �ذ�
    [PunRPC]
    public void Activate()
    {
        _damageZone.SetActive(true);
    }
}

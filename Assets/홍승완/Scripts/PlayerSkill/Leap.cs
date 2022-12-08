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


    public GameObject damageZone;
    GameObject _obstacleDetector;

    #region Private ������

    Quaternion quaternion;
    float elapsedTime;
    Vector3 mouseDir;

    private float damage;
    private float speed;

    #endregion

    private void Awake()
    {
        _obstacleDetector = transform.GetChild(1).gameObject;
    }

    private void OnEnable()
    {
        elapsedTime = 0f;
        damage = Data.Value_1;
        damageZone.GetComponent<SphereCollider>().radius = Data.RangeValue_1;

        damageZone.SetActive(false);
    }

    private void Start()
    {
        if (_ability == null)
        {
            return;
        }

        LookMouseCursor();
        CheckDist();

        _ability.OnLock(true);
        _ani.animator.SetBool("JumpAttack", true);

    }

    Vector3 leapPos;
    private void CheckDist()
    {
        Vector3 mousePos = new Vector3(hit.point.x, _ability.transform.position.y, hit.point.z);

        if (Vector3.Distance(_behaviour.transform.position, mousePos) >= Data.Range)
        {
            Vector3 startPos = _behaviour.transform.position;
            Vector3 endPos = _behaviour.transform.forward;
            leapPos = (startPos + endPos * Data.Range);
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

    bool isArrive;
    private void Update()
    {
        if (photonView.IsMine)
        {
            SkillUpdatePosition();

            // ���ӽð����� �÷��̾ ������ ��ҷ� �̵��Ѵ� => ������ �ִϸ��̼� ó��
            _behaviour.transform.position = Vector3.MoveTowards(_behaviour.transform.position, leapPos, Time.deltaTime * 10f);

            // ���� ��ġ�� ���ư��� �ʵ��� �������� ������������ �����Ѵ�
            _behaviour.ForSkillAgent(leapPos);

            // ������ �ֺ��� �������� �ش�(�ѹ��� ȣ��)
            //Debug.Log($"y�� ���̰� ���ΰ� : {_behaviour.transform.position.y}, {leapPos.y}");
            float diffY = Mathf.Abs(_behaviour.transform.position.y - leapPos.y);
            if (diffY >= 1f)
            {
                _behaviour.transform.position = new Vector3(_behaviour.transform.position.x, leapPos.y, _behaviour.transform.position.z);
            }

            if (Vector3.Distance(_behaviour.transform.position, leapPos) <= 1f)
            {
                //_damageZone.SetActive(true);
                photonView.RPC(nameof(RPC_Activate), RpcTarget.All);
                _ani.animator.SetBool("JumpAttack", false);
                isArrive = true;
            }

            if (isArrive)
            {
                SkillHoldingTime(Data.HoldingTime);
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

        if (elapsedTime >= Data.LockTime)
        {
            _ability.OnLock(false);
        }

        if (elapsedTime >= time)
        {
            if (photonView.IsMine)
            {
                photonView.RPC(nameof(RPC_Activate), RpcTarget.All);
                _ani.animator.SetBool("JumpAttack", false);
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine)
        {
            if (other.GetComponent<Health>() || other.GetComponent<Enemybase>())
            {
                SkillDamage(damage, other.gameObject);
            }
        }
    }

    // TODO : �浹�� ���ڸ����� �������� ó���Ͽ����� ���� ���� ����� �ִ��� �����غ���?
    private void OnCollisionEnter(Collision collision)
    {
        if (photonView.IsMine)
        {
            if (collision.gameObject.tag == "Water" || collision.gameObject.tag == "Ground" || collision.gameObject.tag == _ability.tag && collision.gameObject.layer == 7)
            {
                return;
            }

            _behaviour.transform.forward = mouseDir;
            _behaviour.transform.position = transform.position;
            _behaviour.ForSkillAgent(transform.position);
            photonView.RPC(nameof(RPC_Activate), RpcTarget.All);
            _ani.animator.SetBool("JumpAttack", false);
            isArrive = true;
        }
    }

    // ������ �������� ����Ʈ ����ȭ ����(����Ʈĳ���Ϳ��� ����Ʈ Ȱ��ȭ �ȵǴ���) => RPC�� �ذ�
    [PunRPC]
    public void RPC_Activate()
    {
        Debug.Log("effect on");
        damageZone.SetActive(true);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

}

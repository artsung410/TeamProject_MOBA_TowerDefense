using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smash : SkillHandler
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    // TODO : 부채꼴 60도의 범위로 피해주기

    #region private 변수모음

    Quaternion quaternion;
    float elapsedTime;
    string enemyTag;
    Vector3 mouseDir;

    private float HoldingTime;
    private float Damage;
    private float Range;

    #endregion

    private void OnEnable()
    {
        elapsedTime = 0f;
        Damage = SetDamage;
        HoldingTime = SetHodingTime;
        Range = SetRange;
    }

    void Start()
    {
        if (_ability == null)
        {
            return;
        }

        LookMouseCursor();
        TagProcessing(_ability);
    }





    public void LookMouseCursor()
    {
        // 마우스 방향에서 사용
        RaycastHit hit;
        if (Physics.Raycast(_behaviour.ray, out hit))
        {
            mouseDir = new Vector3(hit.point.x, _ability.transform.position.y, hit.point.z) - _ability.transform.position;

            _ability.transform.forward = mouseDir;
            // 스킬쓸때 플레이어 위치를 그곳으로 고정시키기 위해사용
            quaternion = _ability.transform.localRotation;
        }

    }

    private void TagProcessing(HeroAbility ability)
    {

        if (ability.CompareTag("Blue"))
        {
            enemyTag = "Red";
            //Debug.Log(enemyTag);
        }
        else if (ability.CompareTag("Red"))
        {
            enemyTag = "Blue";
            //Debug.Log(enemyTag);

        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            SkillUpdatePosition();
            SkillHoldingTime(HoldingTime);
        }
    }

    public override void SkillHoldingTime(float time)
    {
        elapsedTime += Time.deltaTime;

        // 지속시간동안 플레이어는 못움직인다
        _stat.MoveSpeed = 0f;

        if (elapsedTime >= time)
        {
            // 사용하기전 이동속도로 돌아온다
            _stat.MoveSpeed = 15f;

            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    public override void SkillUpdatePosition()
    {
        transform.position = _behaviour.transform.position;
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

    bool isCollision;
    public void SectorCalc(Transform target, float radius, float angleRange)
    {
        Vector3 interV = target.position - transform.position;

        if (interV.magnitude <= radius)
        {
            float dot = Vector3.Dot(interV.normalized, transform.forward);
            float theta = Mathf.Acos(dot);
            float degree = Mathf.Rad2Deg * theta;

            if (degree <= angleRange / 2f)
                isCollision = true;
            else
                isCollision = false;
        }
        else
            isCollision = false;
    }
}

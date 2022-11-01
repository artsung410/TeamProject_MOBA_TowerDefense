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

    private void OnEnable()
    {
        elapsedTime = 0f;
        Damage = SetDamage;
        HoldingTime = SetHodingTime;
        Range = SetRange;

    }

    private void Start()
    {
        if (_ability == null)
        {
            return;
        }

        TagProcessing(_ability);
        LookMouseCursor();
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
        SkillUpdatePosition();
        SkillHoldingTime(HoldingTime);
    }

    public override void SkillUpdatePosition()
    {
        // 마우스 위치에서 생성
        transform.position = hit.point;
    }

    public override void SkillDamage(float damage, GameObject target)
    {
        throw new System.NotImplementedException();
    }


    public override void SkillHoldingTime(float time)
    {
        elapsedTime += Time.deltaTime;

        // 지속시간동안 플레이어가 지정한 장소로 도약한다

        if (elapsedTime >= time)
        {
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum Star
{
    OneStar,
    TwoStar,
    ThreeStar,
}


public class SpritSword : SkillHandler
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    #region Private 변수들
    
    Quaternion quaternion;
    float elapsedTime;
    Vector3 mouseDir;

    #endregion

    private float holdingTime;
    private float damage;
    private float range;
    private float lockTime;

    public float Speed;

    //public ScriptableObject temp;
    public Star Star;

    private void OnEnable()
    {
        elapsedTime = 0f;
        damage = SetDamage;
        holdingTime = SetHodingTime;
        range = SetRange;
        lockTime = SetLockTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_ability == null)
        {
            return;
        }

        LookMouseCursor();
        StartCoroutine(SkillLock());
    }

    IEnumerator SkillLock()
    {
        _ability.OnLock(true);
        yield return new WaitForSeconds(lockTime);
        _ability.OnLock(false);
    }

    public void LookMouseCursor()
    {
        // 마우스 방향에서 사용
        RaycastHit hit;
        if (Physics.Raycast(_behaviour.ray, out hit))
        {
            mouseDir = new Vector3(hit.point.x, _ability.transform.position.y, hit.point.z) - _ability.transform.position;

            _ability.transform.forward = mouseDir;
            quaternion = _ability.transform.localRotation;
        }
    }

    void Update()
    {
        if (_ability == null)
        {
            return;
        }

        if (photonView.IsMine)
        {
            SkillUpdatePosition();
            SkillHoldingTime(holdingTime);
        }
    }

    public override void SkillUpdatePosition()
    {
        // 발사체는 앞으로 날아가게끔 한다
        transform.Translate(Time.deltaTime * Speed * Vector3.forward);

        // 회전부분은 처음회전위치에서 날아간다
        transform.rotation = quaternion;
    }


    public override void SkillHoldingTime(float time)
    {
        elapsedTime += Time.deltaTime;

        // 검기 지속시간이 끝나면 사라지게한다
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
            //if (other.CompareTag(enemyTag))
            //{
            //    SkillDamage(damage, other.gameObject);
            //}
            if (other.GetComponent<Health>() || other.GetComponent<Enemybase>())
            {
                SkillDamage(damage, other.gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}

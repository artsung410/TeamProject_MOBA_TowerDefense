using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class SpritSword : SkillHandler
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public GameObject damageZone;

    #region Private 변수들
    
    Quaternion quaternion;
    float elapsedTime;
    Vector3 mouseDir;
    Vector3 currentPos;
    #endregion

    private float damage;
    private float width;
    private float vertical;
    

    private void Awake()
    {
        width = damageZone.GetComponent<BoxCollider>().size.x;
        vertical = damageZone.GetComponent<BoxCollider>().size.z;
    }

    private void OnEnable()
    {
        elapsedTime = 0f;

        damage = Data.Value_1;

        width = Data.RangeValue_1;
        vertical = Data.RangeValue_2;
        
        currentPos = transform.position;

        speed = Data.Value_2;
        if (speed == 0)
        {
            speed = 20f;
        }

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
        yield return new WaitForSeconds(Data.LockTime);
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
            SkillHoldingTime(Data.HoldingTime);
        }
    }

    public override void SkillUpdatePosition()
    {
        Debug.Log(Data.Value_2);
        // 발사체는 앞으로 날아가게끔 한다
        transform.Translate(Time.deltaTime * speed * Vector3.forward);

        // 회전부분은 처음회전위치에서 날아간다
        transform.rotation = quaternion;

        // 사거리까지 날아가면 삭제
        if (Vector3.Distance(currentPos, transform.position) >= Data.Range)
        {
            _ability.OnLock(false);
            PhotonNetwork.Destroy(gameObject);
        }
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

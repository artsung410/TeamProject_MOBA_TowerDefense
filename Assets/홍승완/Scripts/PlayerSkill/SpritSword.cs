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

    #region Private 변수들
    
    Quaternion quaternion;
    float elapsedTime;
    string enemyTag;
    Vector3 mouseDir;

    #endregion

    public float HoldingTime;
    public float Damage;
    public float Range;

    public float Speed;

    private void OnEnable()
    {
        elapsedTime = 0f;
        Damage = SetDamage;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_ability == null)
        {
            return;
        }

        TagProcessing(_ability);
        LookMouseCursor();
    }

    // Update is called once per frame
    void Update()
    {
        SkillUpdatePosition();
        SkillHoldingTime(HoldingTime);
    }

    public void SkillUpdatePosition()
    {
        // 발사체는 앞으로 날아가게끔 한다
        transform.Translate(Time.deltaTime * Speed * Vector3.forward);

        // 회전부분은 처음회전위치에서 날아간다
        transform.rotation = quaternion;
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

    public override void SkillDamage(float damage, GameObject target)
    {
        if (target.gameObject.layer == 7)
        {
            Health player = target.GetComponent<Health>();

            if (player != null)
            {
                player.OnDamage(damage);
            }
        }
        else if (target.gameObject.layer == 8)
        {
            Enemybase minion = target.GetComponent<Enemybase>();

            if (minion != null)
            {
                minion.TakeDamage(damage);
            }
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
            if (other.CompareTag(enemyTag))
            {
                SkillDamage(Damage, other.gameObject);
            }
        }
    }
}

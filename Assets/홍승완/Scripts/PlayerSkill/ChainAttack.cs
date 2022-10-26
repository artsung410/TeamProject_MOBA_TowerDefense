using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChainAttack : SkillHandler
{

    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public ParticleSystem effect;

    #region private 변수모음

    Quaternion quaternion;
    float elapsedTime;
    string enemyTag;
    Vector3 mouseDir;

    #endregion

    public float HoldingTime;
    public float Damage;
    public float Range;

    private void Awake()
    {
        effect = gameObject.GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        elapsedTime = 0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_ability == null)
        {
            return;
        }

        

        // 마우스 방향에서 사용
        RaycastHit hit;
        if (Physics.Raycast(_behaviour.ray, out hit))
        {
            mouseDir = new Vector3(hit.point.x, _ability.transform.position.y, hit.point.z) - _ability.transform.position;

            _ability.transform.forward = mouseDir;
            // 스킬쓸때 플레이어 위치를 그곳으로 고정시키기 위해사용
            quaternion = _ability.transform.localRotation;
        }

        //photonView.RPC(nameof(TagProcessing), RpcTarget.All);
        TagProcessing(_ability);

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

    float dispersionTime = 0f;
    bool isDamage = true;
    // Update is called once per frame
    void Update()
    {
        if (_ability == null)
        {
            return;
        }


        SkillUpdatePosition();

        SkillHoldingTime(HoldingTime);

        dispersionTime += Time.deltaTime;
        float tickTime = HoldingTime / 10;
        if (dispersionTime >= tickTime)
        {
            dispersionTime = 0f;
            isDamage = true;
        }

    }

    /// <summary>
    /// 스킬이 플레이어를 따라오게해줌
    /// </summary>
    private void SkillUpdatePosition()
    {
        transform.position = _ability.transform.position;
        transform.rotation = quaternion;
    }

    private void OnTriggerStay(Collider other)
    {
        // 데미지 두번들어가던부분 IsMine으로 처리
        if (photonView.IsMine)
        {

            if (other.CompareTag(enemyTag))
            {
                if (isDamage)
                {
                    isDamage = false;
                    float tickDamage = Damage / 10;
                    SkillDamage(tickDamage, other.gameObject);
                    //cnt++;
                    //Debug.Log(cnt);
                }
            }
        }
    }

    public override void SkillHoldingTime(float time)
    {
        elapsedTime += Time.deltaTime;

        // 지속시간동안 플레이어가 느려진다
        _stat.MoveSpeed = 3f;

        // 지속시간동안 플레이어는 스킬방향만 바라본다
        _ability.transform.rotation = quaternion;

        // 지속시간이 끝나면 사라진다
        if (elapsedTime >= time)
        {
            _stat.MoveSpeed = 10f;
            // Failed to 'network-remove' GameObject. Client is neither owner nor masterClient taking over for owner who left 오류발생
            // 해결법 : photonView.IsMine 조건 추가
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
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

}

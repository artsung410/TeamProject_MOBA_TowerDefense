using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Blink : SkillHandler
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public GameObject ObstacleDetection;
    public GameObject DamageZone;
    public GameObject BlinkVFX;
    public ParticleSystem kaboom;

    Quaternion quaternion;
    float elasedTiem;
    Vector3 mouseDir;

    private float damage;
    private float speed;

    private void Awake()
    {
        DamageZone.GetComponent<SphereCollider>().radius = Data.RangeValue_1;
    }

    Vector3 vfxPoint;
    private void OnEnable()
    {
        elasedTiem = 0f;

        vfxPoint = transform.position;
        damage = Data.Value_1;
        DamageZone.SetActive(false);
    }

    private IEnumerator Start()
    {
        bool success = false;
        while (!success)
        {
            try
            {
                LookMouseCursor();
                CheckBlinkArrivalPoint();
                _ability.OnLock(true);
                _ability.CharactorRenderEvent(false);
                yield break;
            }
            catch (System.Exception ie)
            {
                print(ie);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    RaycastHit hit;
    public void LookMouseCursor()
    {
        if (Physics.Raycast(_behaviour.ray, out hit))
        {
            mouseDir = new Vector3(hit.point.x, _behaviour.transform.position.y, hit.point.z) - _behaviour.transform.position;

            _behaviour.transform.forward = mouseDir;
            quaternion = _behaviour.transform.localRotation;
        }
    }

    Vector3 arrivalPoint;
    private void CheckBlinkArrivalPoint()
    {
        Vector3 mousePos = new Vector3(hit.point.x, _behaviour.transform.position.y, hit.point.z);
        if (Vector3.Distance(_behaviour.transform.position, mousePos) >= Data.Range)
        {
            Vector3 startPos = _behaviour.transform.position;
            Vector3 endPos = _behaviour.transform.forward;
            arrivalPoint = startPos + endPos * Data.Range;
        }
        else
        {
            arrivalPoint = mousePos;
        }
    }

    bool isArrive;
    private void Update()
    {
        if (photonView.IsMine)
        {
            BlinkVFX.transform.position = vfxPoint;

            SkillUpdatePosition();

            _behaviour.transform.position = Vector3.MoveTowards(transform.position, arrivalPoint, Time.deltaTime * 1000f);

            float dist = Vector3.Distance(_behaviour.transform.position, arrivalPoint);

            // 거리가 가까워지면
            if (dist <= 0.01f)
            {
                isArrive = true;
            }

            if (isArrive)
            {
                StartCoroutine(Temp());

                SkillHoldingTime(Data.HoldingTime);
            }

        }
    }

    IEnumerator Temp()
    {
        yield return new WaitForSeconds(0.3f);
        _behaviour.transform.forward = mouseDir;
        photonView.RPC(nameof(RPC_BlinkDamageActivate), RpcTarget.All);
        _behaviour.ForSkillAgent(arrivalPoint);
        _ability.CharactorRenderEvent(true);
    }

    public override void SkillHoldingTime(float time)
    {
        elasedTiem += Time.deltaTime;

        if (elasedTiem >= Data.LockTime)
        {
            _ability.OnLock(false);
        }

        if (elasedTiem >= time)
        {
            if (photonView.IsMine && PhotonNetwork.IsConnected == true)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    public override void SkillUpdatePosition()
    {
        // 충돌처리용 콜라이더도 같이 움직여야하므로 필요함
        transform.forward = _behaviour.transform.forward;
        transform.position = _behaviour.transform.position;
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
            photonView.RPC(nameof(RPC_BlinkDamageActivate), RpcTarget.All);
            _ability.CharactorRenderEvent(true);
            isArrive = true;
        }
    }

    [PunRPC]
    public void RPC_BlinkDamageActivate()
    {
        if (!kaboom.isPlaying)
        {
            kaboom.Play();
        }
        //Debug.Log("데미지존 켜짐");
        DamageZone.SetActive(true);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}

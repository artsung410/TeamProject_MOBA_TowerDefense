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

    #region Private ������
    
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
        // ���콺 ���⿡�� ���
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
        // �߻�ü�� ������ ���ư��Բ� �Ѵ�
        transform.Translate(Time.deltaTime * speed * Vector3.forward);

        // ȸ���κ��� ó��ȸ����ġ���� ���ư���
        transform.rotation = quaternion;

        // ��Ÿ����� ���ư��� ����
        if (Vector3.Distance(currentPos, transform.position) >= Data.Range)
        {
            _ability.OnLock(false);
            PhotonNetwork.Destroy(gameObject);
        }
    }


    public override void SkillHoldingTime(float time)
    {
        elapsedTime += Time.deltaTime;

        // �˱� ���ӽð��� ������ ��������Ѵ�
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        elapsedTime = 0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

    }
}

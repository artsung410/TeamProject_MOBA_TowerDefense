using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Blaze : SkillHandler
{


    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public GameObject DamageZone;

    Quaternion quaternion;
    float elapsedTime;
    string enemyTag;
    Vector3 mouseDir;

    private float HoldingTime;
    private float Damage;
    private float Range;
    private float Speed;

    float width;
    float length;
    float zCenter;

    private void Awake()
    {
        width = DamageZone.GetComponent<BoxCollider>().size.x;
        length = DamageZone.GetComponent<BoxCollider>().size.z;
        zCenter = DamageZone.GetComponent<BoxCollider>().center.z;
        
    }

    private void OnEnable()
    {
        elapsedTime = 0f;
        Damage = SetDamage;
        HoldingTime = SetHodingTime;
        Range = SetRange;

        width = 12f;
        length = 3;
        zCenter = width / 2f;
    }

    // Start is called before the first frame update
    void Start()
    {
        try
        {

        }
        catch (System.Exception)
        {

            throw;
        }
    }

    private void TagAssignment()
    {
        if (_ability.CompareTag("Blue"))
        {
            enemyTag = "Red";
        }
        else
        {
            enemyTag = "Blue";
        }
    }

    private void LookMouseDir()
    {
        RaycastHit hit;
        if (Physics.Raycast(_behaviour.ray, out hit))
        {
            mouseDir = new Vector3(hit.point.x, _behaviour.transform.position.y, hit.point.z) - _behaviour.transform.position;

            _behaviour.transform.forward = mouseDir;
            quaternion = _behaviour.transform.localRotation;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public override void SkillHoldingTime(float time)
    {
        throw new System.NotImplementedException();
    }

    public override void SkillUpdatePosition()
    {
        throw new System.NotImplementedException();
    }
}

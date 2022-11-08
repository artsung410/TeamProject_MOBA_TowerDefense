using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class SpecialMinionSkill : SkillHandler
{

    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################

   // �÷��̾� ��ġ�� ���ƾ��� �Ÿ��� �˾ƾ���
    float speed = 100f;
    float distance = 5f;
    [SerializeField] Transform trDron = null;

    void Start()
    {
        
        //trDron.transform.localPosition = new Vector3(0, 0, 10);
        Debug.Log($"ability :{_ability}");
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (_ability == null) return;
        transform.position = _ability.transform.position;
        OrbitAround();
    }
    public override void SkillHoldingTime(float time) // ���ӽð�
    {
        
    }

    public override void SkillUpdatePosition() // �̰� �ʿ��� ȣȣ 
    {
        
    }
    void OrbitAround()
    {
        photonView.RPC("RPC_OrbitAround", RpcTarget.All);
    }

    [PunRPC]
    void RPC_OrbitAround()
    {
        //trDron.RotateAround(_ability.transform.position, Vector3.down, speed * Time.deltaTime);
        transform.Rotate(Vector3.down * (speed * Time.deltaTime));
    }
}

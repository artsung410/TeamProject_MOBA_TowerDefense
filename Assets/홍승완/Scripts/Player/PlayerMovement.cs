using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    [SerializeField] float RotateSpeed = 0.5f;
    public float RotateVelocity;

    public LayerMask Layer;
    public RaycastHit Hit;

    #region Other Components
    public NavMeshAgent _agent;
    public Camera Cam;
    HeroCombat _heroCombat;

    #endregion

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _heroCombat = GetComponent<HeroCombat>();
    }

    void Start()
    {

    }

    private void Update()
    {
        HeroAliveCheck();

        // ��Ŭ���� �̵�
        if (Input.GetMouseButton(1))
        {

            // ���콺��ġ���� �� raycast�� ��ü�� �´´ٸ�, �װ��� navmesh��������
            if (Physics.Raycast(Cam.ScreenPointToRay(Input.mousePosition), out Hit, Mathf.Infinity, Layer))
            {
                if (Hit.collider.tag == "Ground")
                {
                    MoveOntheGround(Hit);
                }
            }
        }
    }

    
    public void HeroAliveCheck()
    {
        if (_heroCombat.targetedEnemy != null)
        {
            if (_heroCombat.targetedEnemy.GetComponent<HeroCombat>() != null)
            {
                if (_heroCombat.targetedEnemy.GetComponent<HeroCombat>().isHeroAlive)
                {
                    _heroCombat.targetedEnemy = null;
                }

            }
        }
    }

    public void MoveOntheGround(RaycastHit hit)
    {
        // MOVEMENT
        // ��Ŭ���� ������ ������
        _agent.SetDestination(hit.point);
        _heroCombat.targetedEnemy = null;
        _agent.stoppingDistance = 0;

        // ROTATION
        Quaternion lookAtTheTarget = Quaternion.LookRotation(hit.point - transform.position);
        // ĳ���Ͱ� Ÿ���� ���� ȸ���ϴ� �ӵ� ����
        // RotateSpeed���� �������� ������ ȸ����
        float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, lookAtTheTarget.eulerAngles.y, ref RotateVelocity, RotateSpeed * (Time.deltaTime * 5));

        transform.eulerAngles = new Vector3(0, rotationY, 0);
    }
    
}

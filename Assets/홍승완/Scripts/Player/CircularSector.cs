using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; // OnDrawGizmos

public class CircularSector : MonoBehaviour
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    // 캐릭터 부채꼴모양 표시하게끔 해주는 스크립트
    // 추후 수정할 예정
    //////////////////////////////////////////////////


    public GameObject target; // 부채꼴범위내에서 인식해야할 타겟
    public float angleRange;
    public float radius;

    Color _blue = new Color(0f, 0f, 1f, 0.2f);
    Color _red = new Color(1f, 0f, 0f, 0.2f);

    public bool isCollision = false;

    private void Update()
    {
        //TargetOnSlashScope(target);
    }

    /// <summary>
    /// 
    /// </summary>
    private void TargetOnSlashScope(GameObject target)
    {
        // 나와 타겟사이의 벡터
        Vector3 interV = target.transform.position - transform.position;

        // target과 나 사이의 거리가 radius보다 작으면
        if (interV.magnitude <= radius)
        {
            // '타겟-나 벡터'와 '정면 벡터'를 내적
            float dot = Vector3.Dot(interV.normalized, transform.forward);
            // 두 벡터 모두 단위 벡터이므로 내적결과에 cos의 역을 취해 theta를 구함
            float theta = Mathf.Acos(dot);
            // angleRange와 비교하기위해 degree로 변환
            float degree = Mathf.Rad2Deg * theta;

            // 시야각 판별
            if (degree <= angleRange / 2f)
            {
                isCollision = true;
                // 데미지를 입히는 함수를 호출해준다
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    Debug.Log("슬래쉬 hit");
                }
            }
            else
            {
                isCollision = false;
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    Debug.Log("감나빗");
                }
            }
        }
    }

    

    // 유니티에서 제공해주는 메소드
    // 부채꼴을 그리기 위해 사용
    //private void OnDrawGizmos()
    //{
    //    Handles.color = isCollision ? _red : _blue;

    //    // DrawSolidArc(시작점, 노멀(법선)벡터, 그려줄 방향 벡터, 각도, 반지름)
    //    Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, angleRange/2, radius);
    //    Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -angleRange/2, radius);
    //}

}

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

    // ĳ���� ��ä�ø�� ǥ���ϰԲ� ���ִ� ��ũ��Ʈ
    // ���� ������ ����
    //////////////////////////////////////////////////


    public GameObject target; // ��ä�ù��������� �ν��ؾ��� Ÿ��
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
        // ���� Ÿ�ٻ����� ����
        Vector3 interV = target.transform.position - transform.position;

        // target�� �� ������ �Ÿ��� radius���� ������
        if (interV.magnitude <= radius)
        {
            // 'Ÿ��-�� ����'�� '���� ����'�� ����
            float dot = Vector3.Dot(interV.normalized, transform.forward);
            // �� ���� ��� ���� �����̹Ƿ� ��������� cos�� ���� ���� theta�� ����
            float theta = Mathf.Acos(dot);
            // angleRange�� ���ϱ����� degree�� ��ȯ
            float degree = Mathf.Rad2Deg * theta;

            // �þ߰� �Ǻ�
            if (degree <= angleRange / 2f)
            {
                isCollision = true;
                // �������� ������ �Լ��� ȣ�����ش�
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    Debug.Log("������ hit");
                }
            }
            else
            {
                isCollision = false;
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    Debug.Log("������");
                }
            }
        }
    }

    

    // ����Ƽ���� �������ִ� �޼ҵ�
    // ��ä���� �׸��� ���� ���
    //private void OnDrawGizmos()
    //{
    //    Handles.color = isCollision ? _red : _blue;

    //    // DrawSolidArc(������, ���(����)����, �׷��� ���� ����, ����, ������)
    //    Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, angleRange/2, radius);
    //    Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -angleRange/2, radius);
    //}

}

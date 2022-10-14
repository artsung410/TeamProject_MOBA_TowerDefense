using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    [Header("��ġ ������ �� �÷�")]
    public Color hoverColor;

    [Header("��ġ �Ұ����� �� �÷�")]
    public Color notEnoughMoneyColor;

    [Header("�������κ��� �󸶳� �������� �Ǽ��Ұ���. (y�� ����)")]
    public Vector3 positionOffest;

    [Header("Optional")]
    public GameObject turret; // ���� ���� ���������� �Ǽ� �� Ÿ��

    private Renderer rend;
    private Color startColor;

    BuildManager buildManager;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;

        buildManager = BuildManager.instance;
    }

    // ��ġ �����۾�
    public Vector3 GetBuildPosition()
    {
        return transform.position + positionOffest;
    }

    // <��带 Ŭ������ ��>
    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        // �������� Ÿ���� ������ ���� �ʾ��� ���.
        if (!buildManager.CanBuild)
        {
            return;
        }

        // �̹� ������������ ��ġ �Ұ���
        if (turret != null)
        {
            Debug.Log("Can't build there! - TODO: Display on screen.");
            return;
        }

        // �ڱ� �ڽ��� ������ ��ġ��. (Node�� �ʵ��� GetBuildPosition()���� ȣ���ؼ� ������ ���̿� ��ġ)
        buildManager.BuildTurretOn(this);
    }

    // <��忡 ���콺�� �÷������� ��>
    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (!buildManager.CanBuild)
        {
            return;
        }

        if (buildManager.HasMoney)
        {
            rend.material.color = hoverColor;
        }
        else
        {
            rend.material.color = notEnoughMoneyColor;

        }
    }

    // <��忡�� ���콺�� ����� ��>
    private void OnMouseExit()
    {
        rend.material.color = startColor;
    }
}

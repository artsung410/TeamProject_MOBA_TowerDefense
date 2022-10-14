using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    [Header("설치 가능할 때 컬러")]
    public Color hoverColor;

    [Header("설치 불가능할 때 컬러")]
    public Color notEnoughMoneyColor;

    [Header("지상으로부터 얼마나 떨어져서 건설할건지. (y값 조정)")]
    public Vector3 positionOffest;

    [Header("Optional")]
    public GameObject turret; // 지형 위에 실질적으로 건설 된 타워

    private Renderer rend;
    private Color startColor;

    BuildManager buildManager;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;

        buildManager = BuildManager.instance;
    }

    // 설치 보간작업
    public Vector3 GetBuildPosition()
    {
        return transform.position + positionOffest;
    }

    // <노드를 클릭했을 때>
    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        // 상점에서 타워가 선택이 되지 않았을 경우.
        if (!buildManager.CanBuild)
        {
            return;
        }

        // 이미 지어진곳에는 설치 불가능
        if (turret != null)
        {
            Debug.Log("Can't build there! - TODO: Display on screen.");
            return;
        }

        // 자기 자신의 지형에 설치함. (Node의 필드인 GetBuildPosition()까지 호출해서 적당한 높이에 설치)
        buildManager.BuildTurretOn(this);
    }

    // <노드에 마우스를 올려놓았을 때>
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

    // <노드에서 마우스가 벗어났을 때>
    private void OnMouseExit()
    {
        rend.material.color = startColor;
    }
}

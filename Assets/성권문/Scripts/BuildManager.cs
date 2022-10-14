using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one BuildManager in scene!");
            return;
        }

        instance = this;
    }

    // 타워 건설 효과
    [Header("타워 건설 이펙트")]
    public GameObject buildEffect;

    // 임시로 상점에서 선택된 터렛 데이터를 저장
    private TurretBlueprint turretToBuild;


    // 상점에서 타워가 선택이 되었으면 turretToBuild는 존재해서 true, 선택이 안되었으면 false를 반환
    public bool CanBuild 
    {
        get
        {
            return turretToBuild != null;
        }
    }

    // 소지골드가 부족하지 않으면 true, 부족하면 flase를 반환.
    public bool HasMoney
    {
        get
        {
            return PlayerStats.Money >= turretToBuild.cost;
        }
    }

    // ★★★ 터렛 건설.
    public void BuildTurretOn (Node node)
    {
        // 소지골드가 부족하다면, 건설불가
        if (PlayerStats.Money < turretToBuild.cost)
        {
            return;
        }

        // 타워 가격만큼 소지골드 차감
        PlayerStats.Money -= turretToBuild.cost;

        GameObject turret = (GameObject)Instantiate(turretToBuild.prefab, node.GetBuildPosition(), Quaternion.identity);
        node.turret = turret;

        GameObject effect = (GameObject)Instantiate(buildEffect, node.GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);
    }

    // 상점에서 타워가 선택이 되었으면 turretToBuild에 터렛데이터가 담기고, turret설치가 준비됨.
    public void SelectTurretToBuild(TurretBlueprint turret)
    {
        turretToBuild = turret;
    }
}

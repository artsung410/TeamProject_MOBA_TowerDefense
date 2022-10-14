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

    // Ÿ�� �Ǽ� ȿ��
    [Header("Ÿ�� �Ǽ� ����Ʈ")]
    public GameObject buildEffect;

    // �ӽ÷� �������� ���õ� �ͷ� �����͸� ����
    private TurretBlueprint turretToBuild;


    // �������� Ÿ���� ������ �Ǿ����� turretToBuild�� �����ؼ� true, ������ �ȵǾ����� false�� ��ȯ
    public bool CanBuild 
    {
        get
        {
            return turretToBuild != null;
        }
    }

    // ������尡 �������� ������ true, �����ϸ� flase�� ��ȯ.
    public bool HasMoney
    {
        get
        {
            return PlayerStats.Money >= turretToBuild.cost;
        }
    }

    // �ڡڡ� �ͷ� �Ǽ�.
    public void BuildTurretOn (Node node)
    {
        // ������尡 �����ϴٸ�, �Ǽ��Ұ�
        if (PlayerStats.Money < turretToBuild.cost)
        {
            return;
        }

        // Ÿ�� ���ݸ�ŭ ������� ����
        PlayerStats.Money -= turretToBuild.cost;

        GameObject turret = (GameObject)Instantiate(turretToBuild.prefab, node.GetBuildPosition(), Quaternion.identity);
        node.turret = turret;

        GameObject effect = (GameObject)Instantiate(buildEffect, node.GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);
    }

    // �������� Ÿ���� ������ �Ǿ����� turretToBuild�� �ͷ������Ͱ� ����, turret��ġ�� �غ��.
    public void SelectTurretToBuild(TurretBlueprint turret)
    {
        turretToBuild = turret;
    }
}

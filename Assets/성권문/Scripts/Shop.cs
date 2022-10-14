using UnityEngine;

public class Shop : MonoBehaviour
{
    [Header("�ͷ� ������")]
    public TurretBlueprint standardTurret;
    public TurretBlueprint missileLauncher;
    public TurretBlueprint laserBeamer;
    public TurretBlueprint laserRangeTurret;

    BuildManager buildManager;

    private void Start()
    {
        buildManager = BuildManager.instance;
    }

    public void SelectStandardTurret()
    {
        // �����ϸ� node�� turret�� �Ҵ�.
        Debug.Log("Standard Turret Selected");
        buildManager.SelectTurretToBuild(standardTurret);
    }

    public void SelectMissileLauncher()
    {
        Debug.Log("Missile Launcher Selected");
        buildManager.SelectTurretToBuild(missileLauncher);
    }

    public void SelectLaserBeamer()
    {
        Debug.Log("Laser Beamer Selected");
        buildManager.SelectTurretToBuild(laserBeamer);
    }

    public void SelectLaserRangeTurret()
    {
        Debug.Log("Laser RangeTurret Selected");
        buildManager.SelectTurretToBuild(laserRangeTurret);
    }
}

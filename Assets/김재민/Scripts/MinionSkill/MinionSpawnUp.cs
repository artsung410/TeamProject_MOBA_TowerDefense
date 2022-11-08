using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MinionSpawnUp : SkillHandler
{
    private float HoldingTime;
    private float elaspedTime;

    public bool SpawnSkillOn = false;
    public int _waveCount = 3;
    public ScriptableObject buff;


    public override void SkillUpdatePosition()
    {

    }

    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################

    void Start()
    {
        GameObject MinionSpawner = GameObject.FindGameObjectWithTag("MinionSpawner");
        BuffManager.Instance.AddBuff((BuffData)buff);
        BuffManager.Instance.AssemblyBuff();

        if (GetMytag(_ability) == "Blue")
        {
        MinionSpawner.GetComponent<MinionSpawner>().BlueSkillWave = 3;
        MinionSpawner.GetComponent <MinionSpawner>().tag = GetMytag(_ability); 

        }
        else if (GetMytag(_ability) == "Red")
        {
            MinionSpawner.GetComponent<MinionSpawner>().RedSkillWave = 3;
            MinionSpawner.GetComponent<MinionSpawner>().tag = GetMytag(_ability);
        }

    }
    private void Update()
    {
        SkillHoldingTime(60f);
    }

    
    public override void SkillHoldingTime(float time)
    {
        elaspedTime += Time.deltaTime;
        if (elaspedTime >= time)
        {
            
            elaspedTime = 0;
            PhotonNetwork.Destroy(gameObject);

        }

    }

}

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
    public int _SpawnSize = 0;
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
        //BuffManager.Instance.AddBuff((BuffData)buff);
        BuffManager.Instance.AssemblyBuff();
        _waveCount = (int)Data.Value_1;
        _SpawnSize = (int)Data.HoldingTime;

        if (GetMytag(_ability) == "Blue")
        {
            MinionSpawner.GetComponent<MinionSpawner>().BlueSkillWave = _waveCount;
            MinionSpawner.GetComponent<MinionSpawner>().tag = GetMytag(_ability);
            MinionSpawner.GetComponent<MinionSpawner>().BlueSkillWave = _SpawnSize;


        }
        else if (GetMytag(_ability) == "Red")
        {
            MinionSpawner.GetComponent<MinionSpawner>().RedSkillWave = _waveCount;
            MinionSpawner.GetComponent<MinionSpawner>().tag = GetMytag(_ability);
            MinionSpawner.GetComponent<MinionSpawner>().BlueSkillWave = _SpawnSize;
        }

    }



    public override void SkillHoldingTime(float time)
    {
        

    }

}

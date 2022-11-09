using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class StraightAttack : Projectiles
{
    // ###############################################
    //             NAME : ARTSUNG                      
    //             MAIL : artsung410@gmail.com         
    // ###############################################

    //void OnParticleCollision(GameObject other)
    //{
    //    Damage(other.transform);
    //    Debug.Log(other.transform.name + "督銅適 中宜』』』』』』』』』』』』』』』』』』』』』』』』』』』』』』』』』』』』』』』』』』』』』』』』』』』』』』』』』』』』』』』』』』』』』");
    //}

    //private void OnParticleTrigger()
    //{
    //    Debug.Log("督銅適  闘軒暗 ####################################");
    //}

    void OnParticleTrigger()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();

        // particles
        List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
        List<ParticleSystem.Particle> exit = new List<ParticleSystem.Particle>();

        // get
        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        int numExit = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Exit, exit);

        // iterate
        for (int i = 0; i < numEnter; i++)
        {
            ParticleSystem.Particle p = enter[i];
            p.startColor = new Color32(255, 0, 0, 255);
            enter[i] = p;
        }
        for (int i = 0; i < numExit; i++)
        {
            ParticleSystem.Particle p = exit[i];
            p.startColor = new Color32(0, 255, 0, 255);
            exit[i] = p;
        }

        // set
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Exit, exit);
    }
}
